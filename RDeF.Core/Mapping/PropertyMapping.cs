using System.Collections.Generic;
using RDeF.Entities;
using RDeF.Mapping.Providers;

namespace RDeF.Mapping
{
    /// <summary>Describes a property mapping.</summary>
    public class PropertyMapping : StatementMapping, IPropertyMapping
    {
        internal PropertyMapping(IEntityMapping entityMapping, string name, Iri graph, Iri predicate, IConverter valueConverter) : base(graph, predicate)
        {
            EntityMapping = entityMapping;
            Name = name;
            ValueConverter = valueConverter;
        }

        /// <inheritdoc />
        public IEntityMapping EntityMapping { get; }

        /// <inheritdoc />
        public string Name { get; }

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
                propertyMappingProvider.Property.Name,
                propertyMappingProvider.GetGraph(qiriMappings),
                propertyMappingProvider.GetTerm(qiriMappings),
                valueConverter);
        }
    }
}
