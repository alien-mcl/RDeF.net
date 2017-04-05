using System.Collections.Generic;
using System.Linq;
#if NETSTANDARD1_6
using System.Reflection;
#endif
using RDeF.Mapping.Providers;

namespace RDeF.Mapping
{
    internal static class TermMappingProviderExtensions
    {
        internal static IEnumerable<ITermMappingProvider> WithInheritance(this IEnumerable<ITermMappingProvider> mappingProviders)
        {
            foreach (var mappingProvider in mappingProviders)
            {
                var entityMappingProvider = mappingProvider as IEntityMappingProvider;
                if (entityMappingProvider == null)
                {
                    yield return mappingProvider;
                    continue;
                }

                var parentMappings = from someMappingProvider in mappingProviders.OfType<IEntityMappingProvider>()
                                     where entityMappingProvider.EntityType != someMappingProvider.EntityType &&
                                           someMappingProvider.EntityType.IsAssignableFrom(entityMappingProvider.EntityType)
                                     select someMappingProvider;
                foreach (var parentMapping in parentMappings)
                {
                    yield return new EntityMappingProvider(mappingProvider.EntityType, parentMapping);
                    var parentPropertyMappings = from someMappingProvider in mappingProviders.OfType<IPropertyMappingProvider>()
                                                 where entityMappingProvider.EntityType != someMappingProvider.EntityType &&
                                                       someMappingProvider.EntityType.IsAssignableFrom(entityMappingProvider.EntityType)
                                                 select someMappingProvider;
                    foreach (var parentPropertyMapping in parentPropertyMappings)
                    {
                        var collectionMapping = parentPropertyMapping as ICollectionMappingProvider;
                        yield return (collectionMapping != null
                            ? (IPropertyMappingProvider)new CollectionMappingProvider(mappingProvider.EntityType, collectionMapping)
                            : new PropertyMappingProvider(mappingProvider.EntityType, parentPropertyMapping));
                    }
                }

                yield return mappingProvider;
            }
        }
    }
}
