using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RDeF.Mapping;

namespace RDeF.Entities
{
    /// <summary>Describes an abstract entity context.</summary>
    public interface IEntityContext : IDisposable
    {
        /// <summary>Notifies on disposal.</summary>
        event EventHandler Disposed;

        /// <summary>Raised in case a statement with a predicate without a mapping is encountered.</summary>
        /// <remarks>This event should be used to dynamically provide mappings for variable predicates.</remarks>
        event EventHandler<UnmappedPropertyEventArgs> UnmappedPropertyEncountered;

        /// <summary>Gets the mappings repository.</summary>
        IMappingsRepository Mappings { get; }

        /// <summary>Gets an underlying entity source.</summary>
        IReadableEntitySource EntitySource { get; }

        /// <summary>Loads a specified entity.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="iri">The identifier of the entity to be loaded.</param>
        /// <returns>Instance of the entity of a given <paramref name="iri" />.</returns>
        Task<TEntity> Load<TEntity>(Iri iri) where TEntity : IEntity;

        /// <summary>Loads a specified entity.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="iri">The identifier of the entity to be loaded.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Instance of the entity of a given <paramref name="iri" />.</returns>
        Task<TEntity> Load<TEntity>(Iri iri, CancellationToken cancellationToken) where TEntity : IEntity;

        /// <summary>Creates a specified entity.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="iri">The identifier of the entity to be loaded.</param>
        /// <returns>Instance of the entity of a given <paramref name="iri" />.</returns>
        TEntity Create<TEntity>(Iri iri) where TEntity : IEntity;

        /// <summary>Deletes a specified entity.</summary>
        /// <remarks>This operation won't have an immediate effect on the underlying entity source.</remarks>
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
        /// <returns>Task of the operation.</returns>
        Task Commit();
        
        /// <summary>Commits any changes made to the entities and stores them in the underlying data store.</summary>
        /// <param name="onlyTheseResources">Collection entity Iris to commit. All entities will be committed if omitted.</param>
        /// <returns>Task of the operation.</returns>
        Task Commit(IEnumerable<Iri> onlyTheseResources);

        /// <summary>Commits any changes made to the entities and stores them in the underlying data store.</summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task of the operation.</returns>
        Task Commit(CancellationToken cancellationToken);
        
        /// <summary>Commits any changes made to the entities and stores them in the underlying data store.</summary>
        /// <param name="onlyTheseResources">Collection entity Iris to commit. All entities will be committed if omitted.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task of the operation.</returns>
        Task Commit(IEnumerable<Iri> onlyTheseResources, CancellationToken cancellationToken);

        /// <summary>Rollbacks any changes made to the entities.</summary>
        /// <param name="onlyTheseResources">Optional collection entity Iris to commit. All entities will be committed if omitted.</param>
        void Rollback(IEnumerable<Iri> onlyTheseResources = null);
    }
}
