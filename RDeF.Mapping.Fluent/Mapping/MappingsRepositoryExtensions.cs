using RDeF.Entities;
using RDeF.Mapping.Entities;
using RDeF.Mapping.Explicit;

namespace RDeF.Mapping
{
    /// <summary>Provides useful <see cref="IMappingsRepository" /> extensions.</summary>
    public static class MappingsRepositoryExtensions
    {
        /// <summary>Provides an instance of the <see cref="IMappingsRepository" /> that is aware of local mappings of the <paramref name="entity" />.</summary>
        /// <param name="mappingsRepository">Mappings repository to use as fallback.</param>
        /// <param name="entity">Entity owning mappings.</param>
        /// <returns>Instance of the <see cref="IMappingsRepository" />.</returns>
        public static IMappingsRepository IncludingMappingsFor(this IMappingsRepository mappingsRepository, IEntity entity)
        {
            if (mappingsRepository == null)
            {
                return null;
            }

            IExplicitMappings explicitMappings;
            if ((entity == null) || (!EntityContextExtensions.ExplicitMappings.TryGetValue(entity.Context, out explicitMappings)))
            {
                return mappingsRepository;
            }

            return new EntityAwareMappingsRepository(mappingsRepository, explicitMappings, entity.Iri);
        }
    }
}
