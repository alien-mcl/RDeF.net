using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Mapping;
using RDeF.Mapping.Attributes;
using RDeF.Vocabularies;
using RollerCaster;
using RollerCaster.Reflection;

namespace Given_instance_of.DefaultEntityContext_class
{
    public class when_initializing_a_complex_entity : DefaultEntityContextTest
    {
        private static readonly Iri Iri = new Iri("primary");

        private Entity Entity { get; set; }

        private IDictionary<Iri, int> CallCounter { get; set; }

        public override void TheTest()
        {
            Context.Initialize(Entity);
        }

        [Test]
        public void Should_build_ordinals_collection_correctly()
        {
            Entity.ActLike<IComplexEntity>().Ordinals.ShouldBeEquivalentTo(new[] { 1, 2 });
        }

        [Test]
        public void Should_build_related_collection_correctly()
        {
            Entity.ActLike<IComplexEntity>().Related.Should().AllBeAssignableTo<IComplexEntity>().And
                .Subject.Where((related, index) => related.Iri.ToString() == "related" + (index + 1)).Count().Should().Be(2);
        }

        [Test]
        public void Should_build_floats_list_correctly()
        {
            Entity.ActLike<IComplexEntity>().Floats.ShouldBeEquivalentTo(new[] { 1f, 2f });
        }

        [Test]
        public void Should_build_doubles_list_correctly()
        {
            Entity.ActLike<IComplexEntity>().Doubles.ShouldBeEquivalentTo(new[] { 1, 2 });
        }

        [Test]
        public void Should_build_others_list_correctly()
        {
            Entity.ActLike<IComplexEntity>().Other.Should().AllBeAssignableTo<IComplexEntity>().And
                .Subject.Where((other, index) => other.Iri.ToString() == "other" + (index + 1)).Count().Should().Be(2);
        }

        [Test]
        public void Should_initialize_nested_entity_from_collection()
        {
            Entity.ActLike<IComplexEntity>().Related.First().Floats
                .ShouldBeEquivalentTo(new[]
                {
                    1f * CallCounter[Entity.ActLike<IComplexEntity>().Related.First().Iri],
                    2f * CallCounter[Entity.ActLike<IComplexEntity>().Related.First().Iri]
                });
        }

        [Test]
        public void Should_initialize_another_nested_entity_from_collection()
        {
            Entity.ActLike<IComplexEntity>().Related.Last().Floats
                .ShouldBeEquivalentTo(new[]
                {
                    1f * CallCounter[Entity.ActLike<IComplexEntity>().Related.Last().Iri],
                    2f * CallCounter[Entity.ActLike<IComplexEntity>().Related.Last().Iri]
                });
        }

        [Test]
        public void Should_initialize_nested_entity_from_list()
        {
            Entity.ActLike<IComplexEntity>().Other.First().Floats
                .ShouldBeEquivalentTo(new[]
                {
                    1f * CallCounter[Entity.ActLike<IComplexEntity>().Other.First().Iri],
                    2f * CallCounter[Entity.ActLike<IComplexEntity>().Other.First().Iri]
                });
        }

        [Test]
        public void Should_initialize_another_nested_entity_from_list()
        {
            Entity.ActLike<IComplexEntity>().Other.Last().Floats
                .ShouldBeEquivalentTo(new[]
                {
                    1f * CallCounter[Entity.ActLike<IComplexEntity>().Other.Last().Iri],
                    2f * CallCounter[Entity.ActLike<IComplexEntity>().Other.Last().Iri]
                });
        }

        protected override void ScenarioSetup()
        {
            CallCounter = new Dictionary<Iri, int>();
            Entity = new Entity(Iri, Context);
            EntitySource.Setup(instance => instance.Load(It.IsAny<Iri>())).Returns<Iri>(SetupStatements);
            var entityMapping = new Mock<IEntityMapping>(MockBehavior.Strict);
            entityMapping.SetupGet(instance => instance.Type).Returns(typeof(IComplexEntity));
            Mock<ICollectionMapping>[] properties =
            {
                SetupPropertyMapping(entityMapping, "Ordinals"),
                SetupPropertyMapping(entityMapping, "Related"),
                SetupPropertyMapping(entityMapping, "Floats"),
                SetupPropertyMapping(entityMapping, "Doubles"),
                SetupPropertyMapping(entityMapping, "Other")
            };
            MappingsRepository.Setup(instance => instance.FindEntityMappingFor(It.IsAny<IEntity>(), It.IsAny<Iri>(), Iri.DefaultGraph))
                .Returns<IEntity, Iri, Iri>((entity, iri, graph) => entityMapping.Object);
            MappingsRepository.Setup(instance => instance.FindPropertyMappingFor(It.IsAny<IEntity>(), It.IsAny<Iri>(), Iri.DefaultGraph))
                .Returns<IEntity, Iri, Iri>((entity, iri, graph) => properties.Where(property => property.Object.Term == iri).Select(property => property.Object).FirstOrDefault());
            MappingsRepository.Setup(instance => instance.FindPropertyMappingFor(It.IsAny<IEntity>(), It.IsAny<PropertyInfo>()))
                .Returns<IEntity, PropertyInfo>((entity, propertyInfo) => properties.Where(property => property.Object.PropertyInfo == propertyInfo).Select(property => property.Object).FirstOrDefault());
        }

        private Mock<ICollectionMapping> SetupPropertyMapping(Mock<IEntityMapping> entityMapping, string propertyName)
        {
            var property = typeof(IComplexEntity).GetTypeInfo().GetProperty(propertyName);
            var collectionMapping = property.GetCustomAttribute<CollectionAttribute>();
            var result = new Mock<ICollectionMapping>(MockBehavior.Strict);
            var converter = SetupConverter(property);
            result.SetupGet(instance => instance.EntityMapping).Returns(entityMapping.Object);
            result.SetupGet(instance => instance.PropertyInfo).Returns(property);
            result.SetupGet(instance => instance.Name).Returns(propertyName);
            result.SetupGet(instance => instance.Graph).Returns((Iri)null);
            result.SetupGet(instance => instance.ValueConverter).Returns(converter?.Object);
            result.SetupGet(instance => instance.StoreAs)
                .Returns(property.PropertyType.GetGenericTypeDefinition() == typeof(IList<>) ? CollectionStorageModel.LinkedList : CollectionStorageModel.Simple);
            result.SetupGet(instance => instance.Term).Returns(new Iri(collectionMapping.Iri));
            result.SetupGet(instance => instance.Graph).Returns((Iri)null);
            result.SetupGet(instance => instance.ReturnType).Returns(property.PropertyType);
            return result;
        }

        private IEnumerable<Statement> SetupStatements(Iri subject)
        {
            int calls;
            if (!CallCounter.TryGetValue(subject, out calls))
            {
                CallCounter[subject] = calls = (CallCounter.Count > 0 ? CallCounter.Values.Max() + 1 : 1);
            }

            yield return new Statement(subject, rdf.type, new Iri("class1"));
            yield return new Statement(subject, rdf.type, new Iri("class2"));
            yield return new Statement(subject, new Iri("ordinals"), (1 * calls).ToString(), xsd.@int);
            yield return new Statement(subject, new Iri("ordinals"), (2 * calls).ToString(), xsd.@int);
            yield return new Statement(subject, new Iri("floats"), new Iri("_:blank001"));
            yield return new Statement(new Iri("_:blank001"), rdf.first, (1 * calls).ToString(), xsd.@float);
            yield return new Statement(new Iri("_:blank001"), rdf.rest, new Iri("_:blank002"));
            yield return new Statement(new Iri("_:blank002"), rdf.first, (2 * calls).ToString(), xsd.@float);
            yield return new Statement(new Iri("_:blank002"), rdf.rest, rdf.nil);
            yield return new Statement(subject, new Iri("doubles"), new Iri("_:blank011"));
            yield return new Statement(new Iri("_:blank011"), rdf.first, (1 * calls).ToString(), xsd.@double);
            yield return new Statement(new Iri("_:blank011"), rdf.rest, new Iri("_:blank012"));
            yield return new Statement(new Iri("_:blank012"), rdf.first, (2 * calls).ToString(), xsd.@double);
            yield return new Statement(new Iri("_:blank012"), rdf.rest, rdf.nil);
            if (subject == Iri)
            {
                yield return new Statement(subject, new Iri("related"), new Iri("related1"));
                yield return new Statement(subject, new Iri("related"), new Iri("related2"));
                yield return new Statement(subject, new Iri("other"), new Iri("_:blank021"));
                yield return new Statement(new Iri("_:blank021"), rdf.first, new Iri("other1"));
                yield return new Statement(new Iri("_:blank021"), rdf.rest, new Iri("_:blank022"));
                yield return new Statement(new Iri("_:blank022"), rdf.first, new Iri("other2"));
                yield return new Statement(new Iri("_:blank022"), rdf.rest, rdf.nil);
            }
        }

        private Mock<IConverter> SetupConverter(PropertyInfo propertyInfo)
        {
            if (typeof(IEntity).IsAssignableFrom(propertyInfo.PropertyType.GetItemType()))
            {
                return null;
            }

            var result = new Mock<IConverter>(MockBehavior.Strict);
            result.Setup(instance => instance.ConvertFrom(It.IsAny<Statement>()))
                .Returns<Statement>(statement =>
                {
                    if (statement.DataType == xsd.@float)
                    {
                        return Single.Parse(statement.Value, CultureInfo.InvariantCulture);
                    }

                    if (statement.DataType == xsd.@double)
                    {
                        return Double.Parse(statement.Value, CultureInfo.InvariantCulture);
                    }

                    if (statement.DataType == xsd.@int)
                    {
                        return Int32.Parse(statement.Value);
                    }

                    return Context.CreateInternal(statement.Object, false).ActLike(propertyInfo.PropertyType.GetGenericArguments()[0]);
                });

            return result;
        }
    }
}
