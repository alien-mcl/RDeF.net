using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Collections;
using RDeF.Entities;

namespace RDeF.FluentAssertions
{
    internal static class AssertionsExtensions
    {
        internal static void MatchStatements(
            this GenericCollectionAssertions<KeyValuePair<Iri, IEnumerable<Statement>>> assertion,
            IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>> expected)
        {
            foreach (var resultingGraph in assertion.Subject)
            {
                var expectedGraph = expected.FirstOrDefault(graph => graph.Key == resultingGraph.Key);
                expectedGraph.Should().NotBeNull();
                foreach (var resultingStatement in resultingGraph.Value)
                {
                    var expectedStatement = expectedGraph.Value.FirstOrDefault(statement => resultingStatement == statement);
                    expectedStatement.Should().NotBeNull();
                }
            }
        }

        internal static void BeSimilarTo(
            this GenericCollectionAssertions<KeyValuePair<Iri, IEnumerable<Statement>>> assertion,
            IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>> expected)
        {
            assertion.Subject.Count().Should().Be(expected.Count());
            foreach (var resultingGraph in assertion.Subject)
            {
                var expectedGraph = expected.FirstOrDefault(graph => graph.Key == resultingGraph.Key);
                expectedGraph.Should().NotBeNull();
                resultingGraph.Value.Count().Should().Be(expectedGraph.Value.Count());
            }
        }

        internal static void MatchStatementsInAnyGraph(
            this GenericCollectionAssertions<KeyValuePair<Iri, IEnumerable<Statement>>> assertion,
            IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>> expected)
        {
            foreach (var resultingGraph in assertion.Subject)
            {
                foreach (var resultingStatement in resultingGraph.Value)
                {
                    var expectedStatement = (from graph in expected
                                             from statement in graph.Value
                                             where resultingStatement.Subject == statement.Subject &&
                                                resultingStatement.Predicate == statement.Predicate &&
                                                resultingStatement.Object == statement.Object &&
                                                resultingStatement.Value == statement.Value &&
                                                resultingStatement.Language == statement.Language &&
                                                resultingStatement.DataType == statement.DataType
                                             select statement).FirstOrDefault();
                    expectedStatement.Should().NotBeNull();
                }
            }
        }

        internal static void BeSimilarToInAnyGraph(
            this GenericCollectionAssertions<KeyValuePair<Iri, IEnumerable<Statement>>> assertion,
            IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>> expected)
        {
            assertion.Subject.SelectMany(graph => graph.Value).Count().Should().Be(expected.SelectMany(graph => graph.Value).Count());
        }
    }
}
