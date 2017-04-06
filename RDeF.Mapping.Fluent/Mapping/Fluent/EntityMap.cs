using RDeF.Entities;
using RDeF.Mapping.Explicit;

namespace RDeF.Mapping.Mapping.Fluent
{
    /// <summary>Provides a base type for entity maps.</summary>
    /// <typeparam name="TEntity">Type of the entity being mapped.</typeparam>
    public abstract class EntityMap<TEntity> : DefaultExplicitMappingsBuilder<TEntity> where TEntity : IEntity
    {
    }
}
