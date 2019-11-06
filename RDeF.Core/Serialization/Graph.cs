using System;
using System.Collections.Generic;
using RDeF.Entities;

namespace RDeF.Serialization
{
    /// <summary>Provides a simple, in-memory implementation of the RDF graph.</summary>
    public sealed class Graph : IGraph
    {
        /// <summary>Initializes a new instance of the <see cref="Graph" /> class.</summary>
        /// <param name="statements">Default draph statements.</param>
        public Graph(IEnumerable<Statement> statements) : this(Iri.DefaultGraph, statements)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Graph" /> class.</summary>
        /// <param name="iri">Graph Iri.</param>
        /// <param name="statements">Graph statements.</param>
        public Graph(Iri iri, IEnumerable<Statement> statements)
        {
            if (iri == null)
            {
                throw new ArgumentNullException(nameof(iri));
            }

            if (statements == null)
            {
                throw new ArgumentNullException(nameof(statements));
            }

            Iri = iri;
            Statements = statements;
        }

        /// <inheritdoc />
        public Iri Iri { get; }

        /// <inheritdoc />
        public IEnumerable<Statement> Statements { get; }
    }
}
