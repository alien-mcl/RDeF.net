using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RDeF.Entities
{
    /// <summary>Describes an abstract RDF data source.</summary>
    public interface IEntitySource : IReadableEntitySource
    {
        /// <summary>Loads an entity of a given <paramref name="iri" />.</summary>
        /// <param name="iri">Iri of the entity to be loaded.</param>
        /// <returns>Entity loaded.</returns>
        Task<IEntity> Create(Iri iri);

        /// <summary>Loads an entity of a given <paramref name="iri" />.</summary>
        /// <param name="iri">Iri of the entity to be loaded.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Entity loaded.</returns>
        Task<IEntity> Create(Iri iri, CancellationToken cancellationToken);

        /// <summary>Loads an entity of a given <paramref name="iri" />.</summary>
        /// <typeparam name="TEntity">Type of the entity to load.</typeparam>
        /// <param name="iri">Iri of the entity to be loaded.</param>
        /// <returns>Entity loaded.</returns>
        Task<TEntity> Create<TEntity>(Iri iri) where TEntity : IEntity;

        /// <summary>Loads an entity of a given <paramref name="iri" />.</summary>
        /// <typeparam name="TEntity">Type of the entity to load.</typeparam>
        /// <param name="iri">Iri of the entity to be loaded.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Entity loaded.</returns>
        Task<TEntity> Create<TEntity>(Iri iri, CancellationToken cancellationToken) where TEntity : IEntity;

        /// <summary>Commits given changes to the underlying store.</summary>
        /// <param name="deletedEntities">Identifiers of entities to be deleted.</param>
        /// <param name="retractedStatements">Statements retracted.</param>
        /// <param name="addedStatements">Statements added.</param>
        /// <returns>Task of this operation.</returns>
        Task Commit(
            IEnumerable<Iri> deletedEntities,
            IEnumerable<KeyValuePair<IEntity, ISet<Statement>>> retractedStatements,
            IEnumerable<KeyValuePair<IEntity, ISet<Statement>>> addedStatements);

        /// <summary>Commits given changes to the underlying store.</summary>
        /// <param name="deletedEntities">Identifiers of entities to be deleted.</param>
        /// <param name="retractedStatements">Statements retracted.</param>
        /// <param name="addedStatements">Statements added.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task of this operation.</returns>
        Task Commit(
            IEnumerable<Iri> deletedEntities,
            IEnumerable<KeyValuePair<IEntity, ISet<Statement>>> retractedStatements,
            IEnumerable<KeyValuePair<IEntity, ISet<Statement>>> addedStatements,
            CancellationToken cancellationToken);
    }
}
