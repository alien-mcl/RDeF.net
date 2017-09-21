using RDeF.Entities;
using RDeF.Mapping.Explicit;

namespace RDeF.Mapping.Fluent
{
    /// <summary>Provides a base type for entity maps.</summary>
    /// <typeparam name="TEntity">Type of the entity being mapped.</typeparam>
    public abstract class EntityMap<TEntity> : DefaultExplicitMappingsBuilder<TEntity>, IEntityMap
        where TEntity : IEntity
    {
        /// <inheritdoc />
        public abstract void CreateMappings();
    }
}
