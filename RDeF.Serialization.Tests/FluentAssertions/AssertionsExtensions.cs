using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Collections;
using RDeF.Entities;
using RDeF.Serialization;
using RDeF.Vocabularies;

namespace RDeF.FluentAssertions
{
    internal static class AssertionsExtensions
    {
        private static readonly Iri[] DecimalTypes =
        {
            xsd.@decimal,
            xsd.@double,
            xsd.@float
        };

        internal static T After<T>(this T instance, Action<T> action)
        {
            action(instance);
            return instance;
        }

        internal static void MatchStatements(
            this GenericCollectionAssertions<IGraph> assertion,
            IEnumerable<IGraph> expected,
            bool withStrictGraphMatching)
        {
            foreach (var resultingGraph in assertion.Subject)
            {
                foreach (var resultingStatement in resultingGraph.Statements)
                {
                    var expectedStatement = (
                            from graph in expected
                            from statement in graph.Statements
                            where statement.ThatMatches(resultingStatement, withStrictGraphMatching)
                            select statement)
                        .FirstOrDefault();
                    expectedStatement.Should().NotBeNull();
                }
            }
        }

        private static bool ThatMatches(this Statement expectedStatement, Statement resultingStatement, bool withStrictGraphMatching)
        {
            return resultingStatement == expectedStatement
                || (expectedStatement.Subject.Matches(resultingStatement.Subject) &&
                    resultingStatement.Predicate == expectedStatement.Predicate &&
                    expectedStatement.MatchesObjectOrValue(resultingStatement) &&
                    (!withStrictGraphMatching || expectedStatement.Graph == resultingStatement.Graph));
        }

        private static bool Matches(this Iri expectedIri, Iri resultingIri)
        {
            return (resultingIri.IsBlank && expectedIri.IsBlank)
                || (!resultingIri.IsBlank && !expectedIri.IsBlank && resultingIri == expectedIri);
        }

        private static bool MatchesObjectOrValue(this Statement expectedStatement, Statement resultingStatement)
        {
            return (resultingStatement.Object != null && expectedStatement.Object != null && expectedStatement.Object.Matches(resultingStatement.Object))
                || (resultingStatement.DataType == expectedStatement.DataType &&
                    resultingStatement.MatchesValue(expectedStatement) &&
                    resultingStatement.Language == expectedStatement.Language);
        }

        private static bool MatchesValue(this Statement expectedStatement, Statement resultingStatement)
        {
            if (expectedStatement.Value == resultingStatement.Value)
            {
                return true;
            }

            if (DecimalTypes.Contains(expectedStatement.DataType))
            {
                var expectedValue = Decimal.Parse(expectedStatement.Value, NumberStyles.Any, CultureInfo.InvariantCulture);
                var resultingValue = Decimal.Parse(resultingStatement.Value, NumberStyles.Any, CultureInfo.InvariantCulture);
                return expectedValue == resultingValue;
            }

            return false;
        }
    }
}
