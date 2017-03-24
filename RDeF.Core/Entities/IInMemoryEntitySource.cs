using System.Linq;

namespace RDeF.Entities
{
    /// <summary>Describes a queryable <see cref="IEntitySource" />.</summary>
    internal interface IInMemoryEntitySource : IEntitySource, ISerializableEntitySource
    {
        /// <summary>Loads an entity of a given <paramref name="iri" />.</summary>
        /// <param name="iri">Iri of the entity to be loaded.</param>
        /// <param name="entityContext">Entity context requesting the entity source to load a given entity.</param>
        /// <returns>Entity loaded.</returns>
        IEntity Create(Iri iri, IEntityContext entityContext);

        /// <summary>Loads an entity of a given <paramref name="iri" />.</summary>
        /// <typeparam name="TEntity">Type of the entity to load.</typeparam>
        /// <param name="iri">Iri of the entity to be loaded.</param>
        /// <param name="entityContext">Entity context requesting the entity source to load a given entity.</param>
        /// <returns>Entity loaded.</returns>
        TEntity Create<TEntity>(Iri iri, IEntityContext entityContext) where TEntity : IEntity;

        /// <summary>Converts a given entity source into a queryable collection of types <typeparamref name="TEntity" />.</summary>
        /// <typeparam name="TEntity">Type of entities to search for.</typeparam>
        /// <returns>Queryable entity source.</returns>
        IQueryable<TEntity> AsQueryable<TEntity>() where TEntity : IEntity;
    }
}
