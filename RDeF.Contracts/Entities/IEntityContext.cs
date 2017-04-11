using System;
using System.Linq;
using RDeF.Mapping;

namespace RDeF.Entities
{
    /// <summary>Describes an abstract entity context.</summary>
    public interface IEntityContext : IDisposable
    {
        /// <summary>Notifies on disposal.</summary>
        event EventHandler Disposed;

        /// <summary>Gets the mappings repository.</summary>
        IMappingsRepository Mappings { get; }

        /// <summary>Gets an underlying entity source.</summary>
        IReadableEntitySource EntitySource { get; }

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

        /// <summary>Deletes a specified entity.</summary>
        /// <param name="iri">The identifier of the entity to be deleted.</param>
        void Delete(Iri iri);

        /// <summary>Copies a given <paramref name="entity" /> from one <see cref="IEntityContext" /> to this one.</summary>
        /// <remarks>In case a given <paramref name="entity" /> is from the same context, same instance will be returned.</remarks>
        /// <typeparam name="TEntity">Type of the entity to copy.</typeparam>
        /// <param name="entity">Entity to be copied.</param>
        /// <param name="newIri">Optional new Iri to be assigned to the copied <paramref name="entity" />.</param>
        /// <returns>Copied entity referencing this context.</returns>
        TEntity Copy<TEntity>(TEntity entity, Iri newIri = null) where TEntity : IEntity;

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
