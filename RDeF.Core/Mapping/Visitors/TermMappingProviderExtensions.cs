using System.Collections.Generic;
using RDeF.Mapping.Providers;

namespace RDeF.Mapping.Visitors
{
    internal static class TermMappingProviderExtensions
    {
        internal static void Visit(this ITermMappingProvider termMappingProvider, IEnumerable<IMappingProviderVisitor> visitors)
        {
            var dictionaryMappingProvider = termMappingProvider as IDictionaryMappingProvider;
            var collectionMappingProvider = termMappingProvider as ICollectionMappingProvider;
            var propertyMappingProvider = termMappingProvider as IPropertyMappingProvider;
            var entityMappingProvider = termMappingProvider as IEntityMappingProvider;
            foreach (var visitor in visitors)
            {
                if (dictionaryMappingProvider != null)
                {
                    visitor.Visit(dictionaryMappingProvider);
                    continue;
                }

                if (collectionMappingProvider != null)
                {
                    visitor.Visit(collectionMappingProvider);
                    continue;
                }

                if (propertyMappingProvider != null)
                {
                    visitor.Visit(propertyMappingProvider);
                    continue;
                }

                if (entityMappingProvider != null)
                {
                    visitor.Visit(entityMappingProvider);
                }
            }
        }
    }
}
