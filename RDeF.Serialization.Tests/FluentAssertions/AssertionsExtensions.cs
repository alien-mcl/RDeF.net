using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Collections;
using RDeF.Entities;

namespace RDeF.FluentAssertions
{
    internal static class AssertionsExtensions
    {
        internal static void MatchStatements(this GenericCollectionAssertions<KeyValuePair<Iri, IEnumerable<Statement>>> assertion, IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>> expected)
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

        internal static void BeSimilarTo(this GenericCollectionAssertions<KeyValuePair<Iri, IEnumerable<Statement>>> assertion, IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>> expected)
        {
            assertion.Subject.Count().Should().Be(expected.Count());
            foreach (var resultingGraph in assertion.Subject)
            {
                var expectedGraph = expected.FirstOrDefault(graph => graph.Key == resultingGraph.Key);
                expectedGraph.Should().NotBeNull();
                resultingGraph.Value.Count().Should().Be(expectedGraph.Value.Count());
            }
        }
    }
}
