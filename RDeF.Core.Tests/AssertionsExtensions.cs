using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Primitives;
using RDeF.Entities;
using RDeF.Mapping.Attributes;
using RDeF.Vocabularies;

namespace RDeF
{
    internal static class AssertionsExtensions
    {
        internal static void MatchLiteralValueOf(this ObjectAssertions statement, string expectedValue, Iri expectedDataType = null)
        {
            var isStatement = statement.BeOfType<Statement>();
            isStatement.Which.Value.Should().Be(expectedValue);
            isStatement.Which.DataType.Should().Be(expectedDataType);
        }

        internal static void ContainValuesForPropertyOf<TEntity, TValue>(this GenericCollectionAssertions<Statement> assertion, string propertyName, params TValue[] values)
        {
            var property = typeof(TEntity).GetProperty(propertyName);
            var term = new Iri(property.GetCustomAttribute<CollectionAttribute>().Iri);
            var iri = assertion.Subject.First(statement =>
                statement.Subject != null &&
                statement.Predicate == term).Subject;
            foreach (var value in values)
            {
                assertion.Subject.Should().Contain(statement =>
                    statement.Subject == iri &&
                    statement.Predicate == term &&
                    (typeof(TValue).IsValueType ? statement.Value == String.Format(CultureInfo.InvariantCulture, "{0}", value) : statement.Object == ((IEntity)value).Iri));
            }
        }

        internal static void ContainLinkedListForPropertyOf<TEntity, TValue>(this GenericCollectionAssertions<Statement> assertion, string propertyName, params TValue[] values)
        {
            var property = typeof(TEntity).GetProperty(propertyName);
            var term = new Iri(property.GetCustomAttribute<CollectionAttribute>().Iri);
            var iri = assertion.Subject.First(statement =>
                statement.Subject != null &&
                statement.Predicate == term &&
                statement.Object.Should().BeOfType<Iri>().Which.IsBlank).Object;
            foreach (var value in values)
            {
                assertion.Subject.Should().Contain(statement =>
                    statement.Subject == iri &&
                    statement.Predicate == rdf.first &&
                    (typeof(TValue).IsValueType ? statement.Value == String.Format(CultureInfo.InvariantCulture, "{0}", value) : statement.Object == ((IEntity)value).Iri));
                var next = assertion.Subject.First(statement => 
                    statement.Subject == iri &&
                    statement.Predicate == rdf.last &&
                    statement.Object != null && (statement.Object.IsBlank || statement.Object == rdf.nil));
                iri = next.Object;
            }

            iri.Should().Be(rdf.nil);
        }
    }
}
