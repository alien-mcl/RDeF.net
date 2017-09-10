using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RDeF.Entities;

namespace RDeF.Mapping.Explicit
{
    internal class DefaultExplicitMappings : IExplicitMappings
    {
        private static readonly IEnumerable<IEntityMapping> EmptyEnumerable = new List<IEntityMapping>();

        internal IDictionary<Iri, IDictionary<Type, MergingEntityMapping>> ExplicitMappings { get; } =
            new ConcurrentDictionary<Iri, IDictionary<Type, MergingEntityMapping>>();

        /// <inheritdoc />
        public void Set(IEntityMapping entityMapping, Iri owningEntity)
        {
            if (entityMapping != null)
            {
                IDictionary<Type, MergingEntityMapping> entityMappings;
                if (!ExplicitMappings.TryGetValue(owningEntity, out entityMappings))
                {
                    ExplicitMappings[owningEntity] = entityMappings = new ConcurrentDictionary<Type, MergingEntityMapping>();
                }

                MergingEntityMapping currentEntityMapping;
                if (!entityMappings.TryGetValue(entityMapping.Type, out currentEntityMapping))
                {
                    var newEntityMapping = entityMapping as MergingEntityMapping;
                    if (newEntityMapping != null)
                    {
                        entityMappings[entityMapping.Type] = newEntityMapping;
                        return;
                    }

                    entityMappings[entityMapping.Type] = currentEntityMapping = new MergingEntityMapping(entityMapping.Type);
                }

                foreach (var @class in entityMapping.Classes)
                {
                    currentEntityMapping.Classes.Add(@class);
                }

                foreach (var property in entityMapping.Properties)
                {
                    currentEntityMapping.Properties.Add(property);
                }
            }
        }

        /// <inheritdoc />
        public IEntityMapping FindEntityMappingFor(Type type, Iri owningEntity)
        {
            IDictionary<Type, MergingEntityMapping> entityMappings;
            MergingEntityMapping result;
            return (owningEntity != null && type != null && 
                ExplicitMappings.TryGetValue(owningEntity, out entityMappings) &&
                entityMappings.TryGetValue(type, out result) ? result : null);
        }

        /// <inheritdoc />
        public IPropertyMapping FindPropertyMappingFor(PropertyInfo property, Iri owningEntity)
        {
            return (property == null || owningEntity == null
                ? null
                : (from entityMappings in ExplicitMappings
                   where entityMappings.Key == owningEntity
                   from entity in entityMappings.Value
                   where property.DeclaringType.IsAssignableFrom(entity.Key)
                   let entityMapping = entity.Value
                   from propertyMapping in entityMapping.Properties
                   where propertyMapping.Name == property.Name
                   select propertyMapping).FirstOrDefault());
        }

        /// <inheritdoc />
        public IEnumerator<IEntityMapping> GetEnumerator(Iri owningEntity)
        {
            IDictionary<Type, MergingEntityMapping> result;
            return (ExplicitMappings.TryGetValue(owningEntity, out result)
                ? result.Values.GetEnumerator()
                : EmptyEnumerable.GetEnumerator());
        }
    }
}
