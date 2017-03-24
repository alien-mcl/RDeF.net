using System.Collections.Generic;

namespace RDeF.Entities
{
    /// <summary>Describes an abstract RDF data source.</summary>
    public interface IEntitySource : IReadableEntitySource
    {
        /// <summary>Commits given changes to the underlying store.</summary>
        /// <param name="retractedStatements">Statements retracted.</param>
        /// <param name="addedStatements">Statements added.</param>
        void Commit(IEnumerable<KeyValuePair<IEntity, ISet<Statement>>> retractedStatements, IEnumerable<KeyValuePair<IEntity, ISet<Statement>>> addedStatements);
    }
}
