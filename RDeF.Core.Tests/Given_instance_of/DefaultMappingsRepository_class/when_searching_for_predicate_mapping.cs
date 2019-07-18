using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Mapping;
using RDeF.Mapping.Providers;

namespace Given_instance_of.DefaultMappingsRepository_class
{
    [TestFixture]
    public class when_searching_for_predicate_mapping : DefaultMappingsRepositoryTest
    {
        private const string ExpectedProperty = "Name";

        private IEntity Entity { get; set; }

        private IPropertyMapping Result { get; set; }

        public override void TheTest()
        {
            Result = MappingsRepository.FindPropertyMappingFor(Entity, new Iri(ExpectedProperty));
        }

        [Test]
        public void Should_retrieve_the_property_mapping()
        {
            Result.Name.Should().Be(ExpectedProperty);
        }

        [Test]
        public void Should_throw_when_no_predicate_is_given()
        {
            MappingsRepository.Invoking(instance => instance.FindPropertyMappingFor(null, (Iri)null)).ShouldThrow<ArgumentNullException>();
        }

        protected override void ScenarioSetup()
        {
            MappingBuilder.Setup(instance => instance.BuildMappings(It.IsAny<IEnumerable<IMappingSource>>(), It.IsAny<IDictionary<Type, ICollection<ITermMappingProvider>>>()))
                .Returns(new Dictionary<Type, IEntityMapping>()
                {
                    { typeof(IComplexEntity), CreateEntityMapping<IComplexEntity>(new Iri("ComplexEntity")) },
                    { typeof(IProduct), CreateEntityMapping<IProduct>(new Iri("Product")) }
                });
            var entitySource = new Mock<IEntitySource>(MockBehavior.Strict);
            var changeDetector = new Mock<IChangeDetector>(MockBehavior.Strict);
            var context = new Mock<DefaultEntityContext>(
                MockBehavior.Strict,
                entitySource.Object,
                MappingsRepository,
                changeDetector.Object,
                Array.Empty<ILiteralConverter>());
            context.SetupGet(instance => instance.Mappings).Returns(() => MappingsRepository);
            var entity = new Entity(new Iri(), context.Object);
            entity.CastedTypes.Add(typeof(IProduct));
            Entity = entity;
        }

        private IEntityMapping CreateEntityMapping<TEntity>(Iri @class = null)
        {
            var entityMapping = new Mock<IEntityMapping>(MockBehavior.Strict);
            entityMapping.SetupGet(instance => instance.Type).Returns(typeof(TEntity));
            if (@class != null)
            {
                var typeMapping = new Mock<IStatementMapping>(MockBehavior.Strict);
                typeMapping.SetupGet(instance => instance.Term).Returns(@class);
                typeMapping.SetupGet(instance => instance.Graph).Returns((Iri)null);
                entityMapping.SetupGet(instance => instance.Classes).Returns(new[] { typeMapping.Object });
            }
            else
            {
                entityMapping.SetupGet(instance => instance.Classes).Returns(Array.Empty<IStatementMapping>());
            }

            var propertyMapping = new Mock<IPropertyMapping>(MockBehavior.Strict);
            propertyMapping.SetupGet(instance => instance.Name).Returns(ExpectedProperty);
            propertyMapping.SetupGet(instance => instance.Term).Returns(new Iri(ExpectedProperty));
            propertyMapping.SetupGet(instance => instance.Graph).Returns((Iri)null);
            entityMapping.SetupGet(instance => instance.Properties).Returns(new[] { propertyMapping.Object });
            return entityMapping.Object;
        }
    }
}
