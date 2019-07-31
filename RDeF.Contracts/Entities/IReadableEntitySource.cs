using System;
using System.Collections.Generic;
using System.Linq;

namespace RDeF.Entities
{
    /// <summary>Describes an abstract RDF data source that can be read from.</summary>
    public interface IReadableEntitySource
    {
        /// <summary>Raised when a statement is asserted.</summary>
        event EventHandler<StatementEventArgs> StatementAsserted;

        /// <summary>Loads data related to a given resource identified with <paramref name="iri" />.</summary>
        /// <param name="iri">The identifier of the resource to load data for.</param>
        /// <returns>Set of statements related to resource identified with <paramref name="iri" />.</returns>
        IEnumerable<Statement> Load(Iri iri);

        /// <summary>Converts a given entity source into a queryable collection of types <typeparamref name="TEntity" />.</summary>
        /// <typeparam name="TEntity">Type of entities to search for.</typeparam>
        /// <returns>Queryable entity source.</returns>
        IQueryable<TEntity> AsQueryable<TEntity>() where TEntity : IEntity;
    }
}
