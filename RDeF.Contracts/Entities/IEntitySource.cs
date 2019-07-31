using System.Collections.Generic;

namespace RDeF.Entities
{
    /// <summary>Describes an abstract RDF data source.</summary>
    public interface IEntitySource : IReadableEntitySource
    {
        /// <summary>Loads an entity of a given <paramref name="iri" />.</summary>
        /// <param name="iri">Iri of the entity to be loaded.</param>
        /// <returns>Entity loaded.</returns>
        IEntity Create(Iri iri);

        /// <summary>Loads an entity of a given <paramref name="iri" />.</summary>
        /// <typeparam name="TEntity">Type of the entity to load.</typeparam>
        /// <param name="iri">Iri of the entity to be loaded.</param>
        /// <returns>Entity loaded.</returns>
        TEntity Create<TEntity>(Iri iri) where TEntity : IEntity;

        /// <summary>Deletes an entity of a given <paramref name="iri" />.</summary>
        /// <param name="iri">Iri of the entity to be deleted.</param>
        void Delete(Iri iri);

        /// <summary>Commits given changes to the underlying store.</summary>
        /// <param name="deletedEntities">Identifiers of entities to be deleted.</param>
        /// <param name="retractedStatements">Statements retracted.</param>
        /// <param name="addedStatements">Statements added.</param>
        void Commit(
            IEnumerable<Iri> deletedEntities,
            IEnumerable<KeyValuePair<IEntity, ISet<Statement>>> retractedStatements,
            IEnumerable<KeyValuePair<IEntity, ISet<Statement>>> addedStatements);
    }
}
