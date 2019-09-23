using System.Collections.Generic;
using RDeF.Entities;

namespace RDeF.Serialization
{
    /// <summary>Provides a simple, in-memory implementation of the RDF graph.</summary>
    public sealed class Graph : IGraph
    {
        internal Graph(Iri iri, IEnumerable<Statement> statements)
        {
            Iri = iri;
            Statements = statements;
        }

        /// <inheritdoc />
        public Iri Iri { get; }

        /// <inheritdoc />
        public IEnumerable<Statement> Statements { get; }
    }
}
