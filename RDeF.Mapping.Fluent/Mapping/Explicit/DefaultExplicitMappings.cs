using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RDeF.Entities;
using RDeF.Mapping.Reflection;

namespace RDeF.Mapping.Explicit
{
    internal class DefaultExplicitMappings : IExplicitMappings
    {
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
                    var existingMappings = currentEntityMapping.Properties.Where(item => item.Name == property.Name).ToList();
                    foreach (var existingMapping in existingMappings)
                    {
                        currentEntityMapping.Properties.Remove(existingMapping);
                    }

                    currentEntityMapping.Properties.Add(property);
                    foreach (var existingMapping in existingMappings)
                    {
                        currentEntityMapping.Properties.Add(existingMapping);
                    }
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
        public IPropertyMapping FindPropertyMappingFor(Iri term, Iri graph, Iri owningEntity)
        {
            return (term == null || owningEntity == null
                ? null
                : (from entityMappings in ExplicitMappings
                   where entityMappings.Key == owningEntity
                   from entity in entityMappings.Value
                   let entityMapping = entity.Value
                   from propertyMapping in entityMapping.Properties
                   where propertyMapping.PropertyInfo is ExplicitlyMappedPropertyInfo
                   let explicitelyMappedPropertyInfo = (ExplicitlyMappedPropertyInfo)propertyMapping.PropertyInfo
                   where explicitelyMappedPropertyInfo.Predicate == term && explicitelyMappedPropertyInfo.Graph == graph
                   select propertyMapping).FirstOrDefault());
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
                   orderby propertyMapping.PropertyInfo == property ? 1 : 0 descending
                   select propertyMapping).FirstOrDefault());
        }

        /// <inheritdoc />
        public IEnumerator<IEntityMapping> GetEnumerator()
        {
            return (from set in ExplicitMappings.Values
                    from mapping in set.Values
                    select mapping).GetEnumerator();
        }
    }
}
