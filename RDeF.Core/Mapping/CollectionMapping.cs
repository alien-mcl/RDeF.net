using System.Collections.Generic;
using RDeF.Entities;
using RDeF.Mapping.Providers;

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

        internal static CollectionMapping CreateFrom(
            IEntityMapping entityMapping,
            ICollectionMappingProvider collectionMappingProvider,
            IConverter valueConverter,
            IEnumerable<QIriMapping> qiriMappings)
        {
            return new CollectionMapping(
                entityMapping,
                collectionMappingProvider.Property.Name,
                collectionMappingProvider.GetGraph(qiriMappings),
                collectionMappingProvider.GetTerm(qiriMappings),
                valueConverter,
                collectionMappingProvider.StoreAs);
        }
    }
}
