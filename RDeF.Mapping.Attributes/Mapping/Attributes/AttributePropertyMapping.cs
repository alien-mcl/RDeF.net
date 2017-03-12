using System.Diagnostics;
using RDeF.Entities;

namespace RDeF.Mapping.Attributes
{
    /// <summary>Represents an attribute based property mapping.</summary>
    [DebuggerDisplay("{EntityMapping.Type.Name,nq}.{Name,nq} ({Predicate,nq})")]
    public sealed class AttributePropertyMapping : IPropertyMapping
    {
        internal AttributePropertyMapping(IEntityMapping entityMapping, string name, Iri predicate, Iri graph, IConverter converter = null)
        {
            EntityMapping = entityMapping;
            Name = name;
            Predicate = predicate;
            Graph = graph;
            ValueConverter = converter;
        }

        /// <inheritdoc />
        public IEntityMapping EntityMapping { get; }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public Iri Graph { get; }

        /// <inheritdoc />
        public Iri Predicate { get; }

        /// <inheritdoc />
        public IConverter ValueConverter { get; }
    }
}
