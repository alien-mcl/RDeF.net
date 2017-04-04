using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RDeF.Mapping.Explicit
{
    internal class DefaultExplicitMappings : IExplicitMappings
    {
        internal IDictionary<Type, IEntityMapping> ExplicitMappings { get; } = new ConcurrentDictionary<Type, IEntityMapping>();

        /// <inheritdoc />
        public void Set(IEntityMapping entityMapping)
        {
            if (entityMapping != null)
            {
                ExplicitMappings[entityMapping.Type] = entityMapping;
            }
        }

        /// <inheritdoc />
        public IEntityMapping FindEntityMappingFor(Type type)
        {
            IEntityMapping result;
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
