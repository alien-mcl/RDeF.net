using System.Collections.Generic;
using RDeF.Entities;

namespace RDeF.Serialization
{
    /// <summary>Provides an abstraction over an RDF graph.</summary>
    public interface IGraph
    {
        /// <summary>Gets <see cref="Iri" /> of this graph.</summary>
        Iri Iri { get; }

        /// <summary>Gets statements from this graph.</summary>
        IEnumerable<Statement> Statements { get; }
    }
}
