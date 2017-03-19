using RDeF.Entities;

namespace RDeF.Mapping
{
    /// <summary>Describes a statement mapping.</summary>
    public class StatementMapping : IStatementMapping
    {
        internal StatementMapping(Iri graph, Iri predicate)
        {
            Graph = graph;
            Term = predicate;
        }

        /// <inheritdoc />
        public Iri Graph { get; }

        /// <inheritdoc />
        public Iri Term { get; }
    }
}
