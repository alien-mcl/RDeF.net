using System.Collections.Generic;

namespace RDeF.Entities
{
    /// <summary>Describes an abstract RDF data source.</summary>
    public interface IEntitySource
    {
        /// <summary>Loads data related to a given resource identified with <paramref name="iri" />.</summary>
        /// <param name="iri">The identifier of the resource to load data for.</param>
        /// <returns>Set of statements related to resource identified with <paramref name="iri" />.</returns>
        IEnumerable<Statement> Load(Iri iri);

        /// <summary>Commits given changes to the underlying store.</summary>
        /// <param name="retractedStatements">Statements retracted.</param>
        /// <param name="addedStatements">Statements added.</param>
        void Commit(IEnumerable<KeyValuePair<IEntity, ISet<Statement>>> retractedStatements, IEnumerable<KeyValuePair<IEntity, ISet<Statement>>> addedStatements);
    }
}
