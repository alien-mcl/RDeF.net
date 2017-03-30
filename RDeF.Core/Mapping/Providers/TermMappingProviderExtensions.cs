using System;

namespace RDeF.Mapping.Providers
{
    internal static class TermMappingProviderExtensions
    {
        internal static ITermMappingProvider TryCloseGenericTermMappingProvider(this ITermMappingProvider openGenericMappingProvider, Type closedGenericType)
        {
            var openGenericEntityMappingProvider = openGenericMappingProvider as IEntityMappingProvider;
            if (openGenericEntityMappingProvider != null)
            {
                return new ClosedGenericEntityMappingProvider(closedGenericType, openGenericEntityMappingProvider);
            }

            var openGenericCollectionMappingProvider = openGenericMappingProvider as ICollectionMappingProvider;
            if (openGenericCollectionMappingProvider != null)
            {
                return new ClosedGenericCollectionMappingProvider(closedGenericType, openGenericCollectionMappingProvider);
            }

            var openGenericPropertyMappingProvider = openGenericMappingProvider as IPropertyMappingProvider;
            if (openGenericPropertyMappingProvider != null)
            {
                return new ClosedGenericPropertyMappingProvider(closedGenericType, openGenericPropertyMappingProvider);
            }

            return openGenericMappingProvider;
        }
    }
}
