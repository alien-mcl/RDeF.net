using RDeF.Entities;

namespace RDeF.Mapping
{
    /// <summary>Describes a collection mapping.</summary>
    public class CollectionMapping : PropertyMapping, ICollectionMapping
    {
        internal CollectionMapping(IEntityMapping entityMapping, string name, Iri graph, Iri predicate, IConverter valueConverter, CollectionStorageModel storeAs)
            : base(entityMapping, name, graph, predicate, valueConverter)
        {
            StoreAs = storeAs;
        }

        /// <inheritdoc />
        public CollectionStorageModel StoreAs { get; }
    }
}
