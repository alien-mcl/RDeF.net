using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Collections;
using RDeF.Serialization;

namespace RDeF.FluentAssertions
{
    internal static class AssertionsExtensions
    {
        internal static T After<T>(this T instance, Action<T> action)
        {
            action(instance);
            return instance;
        }

        internal static void MatchStatements(
            this GenericCollectionAssertions<IGraph> assertion,
            IEnumerable<IGraph> expected)
        {
            foreach (var resultingGraph in assertion.Subject)
            {
                var expectedGraph = expected.FirstOrDefault(graph => graph.Iri == resultingGraph.Iri);
                expectedGraph.Should().NotBeNull();
                foreach (var resultingStatement in resultingGraph.Statements)
                {
                    var expectedStatement = expectedGraph.Statements.FirstOrDefault(statement => resultingStatement == statement);
                    expectedStatement.Should().NotBeNull();
                }
            }
        }

        internal static void BeSimilarTo(
            this GenericCollectionAssertions<IGraph> assertion,
            IEnumerable<IGraph> expected)
        {
            assertion.Subject.Count().Should().Be(expected.Count());
            foreach (var resultingGraph in assertion.Subject)
            {
                var expectedGraph = expected.FirstOrDefault(graph => graph.Iri == resultingGraph.Iri);
                expectedGraph.Should().NotBeNull();
                resultingGraph.Statements.Count().Should().Be(expectedGraph.Statements.Count());
            }
        }

        internal static void MatchStatementsInAnyGraph(
            this GenericCollectionAssertions<IGraph> assertion,
            IEnumerable<IGraph> expected)
        {
            foreach (var resultingGraph in assertion.Subject)
            {
                foreach (var resultingStatement in resultingGraph.Statements)
                {
                    var expectedStatement = (from graph in expected
                                             from statement in graph.Statements
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
            this GenericCollectionAssertions<IGraph> assertion,
            IEnumerable<IGraph> expected)
        {
            assertion.Subject.SelectMany(graph => graph.Statements).Count()
                .Should().Be(expected.SelectMany(graph => graph.Statements).Count());
        }
    }
}
