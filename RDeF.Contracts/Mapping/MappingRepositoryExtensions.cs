using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace RDeF.Mapping
{
    /// <summary>Provides useful <see cref="IMappingsRepository" /> extenions.</summary>
    public static class MappingRepositoryExtensions
    {
        /// <summary>Gathers all <see cref="IEntityMapping" />s matching a given <typeparamref name="TEntity" />.</summary>
        /// <typeparam name="TEntity">Type for which to find mappings.</typeparam>
        /// <param name="mappingsRepository">Mappings repository to search through.</param>
        /// <returns>Collection of <see cref="IEntityMapping" /> matching a given <typeparamref name="TEntity" />.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Strong typing is essential and no real instance can be provided.")]
        public static IEnumerable<IEntityMapping> FindEntityMappingsFor<TEntity>(this IMappingsRepository mappingsRepository)
        {
            return mappingsRepository?.Where(_ => _.Type.IsAssignableFrom(typeof(TEntity))).ToList()
                ?? (IEnumerable<IEntityMapping>)Array.Empty<IEntityMapping>();
        }
    }
}
