using RDeF.Entities;

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
    }
}
