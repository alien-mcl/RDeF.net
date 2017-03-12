using System.Linq;
using RDeF.Mapping;

namespace RDeF.Entities
{
    /// <summary>Describes an abstract entity context.</summary>
    public interface IEntityContext
    {
        /// <summary>Gets the mappings repository.</summary>
        IMappingsRepository MappingsRepository { get; }

        /// <summary>Loads a specified entity.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="iri">The identifier of the entity to be loaded.</param>
        /// <returns>Instance of the entity of a given <paramref name="iri" />.</returns>
        TEntity Load<TEntity>(Iri iri) where TEntity : IEntity;

        /// <summary>Creates a specified entity.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="iri">The identifier of the entity to be loaded.</param>
        /// <returns>Instance of the entity of a given <paramref name="iri" />.</returns>
        TEntity Create<TEntity>(Iri iri) where TEntity : IEntity;

        /// <summary>Converts a context to be LINQ queryable.</summary>
        /// <typeparam name="TEntity">Type of entities to query for.</typeparam>
        /// <returns>Queryable collection of entities.</returns>
        IQueryable<TEntity> AsQueryable<TEntity>() where TEntity : IEntity;

        /// <summary>Commits any changes made to the entities and stores them in the underlying data store.</summary>
        void Commit();

        /// <summary>Rollbacks any changes made to the entities.</summary>
        void Rollback();
    }
}
