using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RDeF.Mapping.Explicit
{
    internal class DefaultExplicitMappings : IExplicitMappings
    {
        internal IDictionary<Type, MergingEntityMapping> ExplicitMappings { get; } = new ConcurrentDictionary<Type, MergingEntityMapping>();

        /// <inheritdoc />
        public void Set(IEntityMapping entityMapping)
        {
            if (entityMapping != null)
            {
                MergingEntityMapping currentEntityMapping;
                if (!ExplicitMappings.TryGetValue(entityMapping.Type, out currentEntityMapping))
                {
                    var newEntityMapping = entityMapping as MergingEntityMapping;
                    if (newEntityMapping != null)
                    {
                        ExplicitMappings[entityMapping.Type] = newEntityMapping;
                        return;
                    }

                    ExplicitMappings[entityMapping.Type] = currentEntityMapping = new MergingEntityMapping(entityMapping.Type);
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
        public IEntityMapping FindEntityMappingFor(Type type)
        {
            MergingEntityMapping result;
            return (type != null && ExplicitMappings.TryGetValue(type, out result) ? result : null);
        }

        /// <inheritdoc />
        public IPropertyMapping FindPropertyMappingFor(PropertyInfo property)
        {
            return (property == null ? null : (from entity in ExplicitMappings
                    where property.DeclaringType.IsAssignableFrom(entity.Key)
                    let entityMapping = entity.Value
                    from propertyMapping in entityMapping.Properties
                    where propertyMapping.Name == property.Name
                    select propertyMapping).FirstOrDefault());
        }
    }
}
