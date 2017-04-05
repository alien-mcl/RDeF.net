using RDeF.Vocabularies;

namespace RDeF.Entities
{
    internal static class StatementExtensions
    {
        internal static bool IsRelatedTo(this Statement statement, IEntity entity)
        {
            return (statement.Subject == entity.Iri) && (statement.Predicate != rdf.first) && (statement.Predicate != rdf.rest);
        }

        internal static bool Matches(this Statement statement, Iri graph)
        {
            return (graph == null) || (graph == statement.Graph);
        }

        internal static bool IsTypeAssertion(this Statement statement)
        {
            return statement.Predicate == rdf.type;
        }
    }
}
