using System;
using System.Collections.Generic;
using System.Reflection;
using RDeF.Entities;
using RDeF.Mapping.Providers;
using RDeF.Mapping.Visitors;

namespace RDeF.Mapping
{
    internal class CollectionMappingProvider : ICollectionMappingProvider
    {
        private readonly ICollectionMappingProvider _parentCollectionMappingProvider;

        internal CollectionMappingProvider(Type entityType, ICollectionMappingProvider parentCollectionMappingProvider)
        {
            EntityType = entityType;
            _parentCollectionMappingProvider = parentCollectionMappingProvider;
        }

        /// <inheritdoc />
        public Type EntityType { get; }

        /// <inheritdoc />
        public PropertyInfo Property { get { return _parentCollectionMappingProvider.Property; } }

        /// <inheritdoc />
        public Type ValueConverterType
        {
            get { return _parentCollectionMappingProvider.ValueConverterType; }
            set { _parentCollectionMappingProvider.ValueConverterType = value; }
        }

        /// <inheritdoc />
        public CollectionStorageModel StoreAs
        {
            get { return _parentCollectionMappingProvider.StoreAs; }
            set { _parentCollectionMappingProvider.StoreAs = value; }
        }

        /// <inheritdoc />
        public Iri GetGraph(IEnumerable<QIriMapping> qiriMappings)
        {
            return _parentCollectionMappingProvider.GetGraph(qiriMappings);
        }

        /// <inheritdoc />
        public Iri GetTerm(IEnumerable<QIriMapping> qiriMappings)
        {
            return _parentCollectionMappingProvider.GetTerm(qiriMappings);
        }

        /// <inheritdoc />
        public void Accept(IMappingProviderVisitor visitor)
        {
            visitor?.Visit(this);
        }
    }
}
