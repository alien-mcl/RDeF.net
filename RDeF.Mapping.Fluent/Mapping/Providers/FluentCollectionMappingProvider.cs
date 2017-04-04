using System;
using System.Diagnostics;
using System.Reflection;
using RDeF.Entities;
using RDeF.Mapping.Visitors;

namespace RDeF.Mapping.Providers
{
    /// <summary>Represents an attribute based collection mapping.</summary>
    [DebuggerDisplay("{EntityType.Name,nq}.{Property.Name,nq}[] ({Iri.ToString()??Prefix+\":\"+Term,nq})")]
    public sealed class FluentCollectionMappingProvider : FluentPropertyMappingProvider, ICollectionMappingProvider
    {
        internal FluentCollectionMappingProvider(
            Type entityType,
            PropertyInfo property,
            Iri iri,
            CollectionStorageModel storeAs = CollectionStorageModel.Unspecified,
            Type valueConverterType = null,
            Iri graph = null)
            : base(entityType, property, iri, valueConverterType, graph)
        {
            StoreAs = storeAs;
        }

        internal FluentCollectionMappingProvider(
            Type entityType,
            PropertyInfo property,
            string prefix,
            string term,
            CollectionStorageModel storeAs = CollectionStorageModel.Unspecified,
            Type valueConverterType = null,
            Iri graph = null)
            : base(entityType, property, prefix, term, valueConverterType, graph)
        {
            StoreAs = storeAs;
        }

        internal FluentCollectionMappingProvider(
            Type entityType,
            PropertyInfo property,
            string prefix,
            string term, 
            CollectionStorageModel storeAs = CollectionStorageModel.Unspecified,
            Type valueConverterType = null,
            string graphPrefix = null,
            string graphTerm = null)
            : base(entityType, property, prefix, term, valueConverterType, graphPrefix, graphTerm)
        {
            StoreAs = storeAs;
        }

        /// <inheritdoc />
        public CollectionStorageModel StoreAs { get; set; }

        /// <inheritdoc />
        public override void Accept(IMappingProviderVisitor visitor)
        {
            if (visitor == null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            visitor.Visit(this);
        }
    }
}
