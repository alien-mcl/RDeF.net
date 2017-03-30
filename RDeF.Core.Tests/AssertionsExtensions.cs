using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Primitives;
using RDeF.Entities;
using RDeF.Mapping;
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

        internal static void ContainMappingsFor(this GenericCollectionAssertions<IEntityMapping> assertion, Type type)
        {
            assertion.Subject.SingleOrDefault(item => item.Type == type).Should().BeAssignableTo<IEntityMapping>().ContainMappingsFor(type);
        }

        internal static void ContainMappingsFor(this AndWhichConstraint<ObjectAssertions, IEntityMapping> assertion, Type type)
        {
            var entityMapping = assertion.Subject;
            var allTypes = new[] { type }.Union(type.GetInterfaces());
            entityMapping.ContainClassesFor(allTypes);
            entityMapping.ContainPropertiesFor(allTypes);
        }

        private static void ContainClassesFor(this IEntityMapping entityMapping, IEnumerable<Type> allTypes)
        {
            var expectedClasses = allTypes.SelectMany(entityType => entityType.GetCustomAttributes<ClassAttribute>(true));
            var matchedClasses = from @class in entityMapping.Classes
                                 from expectedClass in expectedClasses
                                 where @class.Term == expectedClass.MappedIri && @class.Graph == expectedClass.GraphIri
                                 select @class;
            matchedClasses.Count().Should().Be(expectedClasses.Count());
        }

        private static void ContainPropertiesFor(this IEntityMapping entityMapping, IEnumerable<Type> allTypes)
        {
            var expectedProperties = from entityType in allTypes
                                     from property in entityType.GetProperties()
                                     from attribute in property.GetCustomAttributes(true).OfType<TermAttribute>()
                                     select attribute;
            var matchedProperties = from property in entityMapping.Properties
                                    from expectedProperty in expectedProperties
                                    where property.Term == expectedProperty.MappedIri && property.Graph == expectedProperty.GraphIri
                                    select property;
            matchedProperties.Count().Should().Be(expectedProperties.Count());
        }
    }
}
