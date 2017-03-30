using System;
using RDeF.Mapping.Visitors;

namespace RDeF.Mapping.Providers
{
    internal class ClosedGenericCollectionMappingProvider : ClosedGenericPropertyMappingProvider, ICollectionMappingProvider
    {
        private readonly ICollectionMappingProvider _openGenericCollectionMappingProvider;

        internal ClosedGenericCollectionMappingProvider(Type closedGenericType, ICollectionMappingProvider openGenericCollectionMappingProvider)
            : base(closedGenericType, openGenericCollectionMappingProvider)
        {
            _openGenericCollectionMappingProvider = openGenericCollectionMappingProvider;
        }

        /// <inheritdoc />
        public CollectionStorageModel StoreAs
        {
            get { return _openGenericCollectionMappingProvider.StoreAs; }
            set { _openGenericCollectionMappingProvider.StoreAs = value; }
        }

        /// <inheritdoc />
        public override void Accept(IMappingProviderVisitor visitor)
        {
            visitor?.Visit(this);
        }
    }
}
