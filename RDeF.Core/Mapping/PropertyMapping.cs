using System;
using System.Collections.Generic;
using System.Reflection;
using RDeF.Entities;
using RDeF.Mapping.Providers;

namespace RDeF.Mapping
{
    /// <summary>Describes a property mapping.</summary>
    public class PropertyMapping : StatementMapping, IPropertyMapping
    {
        internal PropertyMapping(IEntityMapping entityMapping, PropertyInfo propertyInfo, Iri graph, Iri predicate, IConverter valueConverter) : base(graph, predicate)
        {
            EntityMapping = entityMapping;
            PropertyInfo = propertyInfo;
            ValueConverter = valueConverter;
        }

        /// <inheritdoc />
        public IEntityMapping EntityMapping { get; }

        /// <inheritdoc />
        public string Name
        {
            get { return PropertyInfo.Name; }
        }

        /// <inheritdoc />
        public Type ReturnType
        {
            get { return PropertyInfo.PropertyType; }
        }

        /// <inheritdoc />
        public PropertyInfo PropertyInfo { get; }

        /// <inheritdoc />
        public IConverter ValueConverter { get; }

        internal static PropertyMapping CreateFrom(
            IEntityMapping entityMapping,
            IPropertyMappingProvider propertyMappingProvider,
            IConverter valueConverter,
            IEnumerable<QIriMapping> qiriMappings)
        {
            return new PropertyMapping(
                entityMapping,
                propertyMappingProvider.Property,
                propertyMappingProvider.GetGraph(qiriMappings),
                propertyMappingProvider.GetTerm(qiriMappings),
                valueConverter);
        }
    }
}
