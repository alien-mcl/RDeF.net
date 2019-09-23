using System.Collections.Generic;
using RDeF.Entities;

namespace RDeF.Serialization
{
    /// <summary>Provides a simple, writeable, in-memory implementation of the RDF graph.</summary>
    public sealed class WritableGraph : IGraph
    {
        internal WritableGraph(Iri iri)
        {
            Iri = iri;
        }

        /// <inheritdoc />
        Iri IGraph.Iri
        {
            get { return Iri; }
        }

        /// <inheritdoc />
        IEnumerable<Statement> IGraph.Statements
        {
            get { return Statements; }
        }

        internal Iri Iri { get; set; }

        internal ICollection<Statement> Statements { get; } = new List<Statement>();
    }
}
