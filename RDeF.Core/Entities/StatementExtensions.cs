using System.Collections.Generic;
using RDeF.Collections;
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

        internal static void EnsureCache(this Statement statement, IDictionary<Iri, ISet<Statement>> subjects)
        {
            subjects.EnsureKey(statement.Subject).Add(statement);
            subjects.EnsureKey(statement.Predicate);
            if (statement.Object != null)
            {
                subjects.EnsureKey(statement.Object);
            }
        }
    }
}
