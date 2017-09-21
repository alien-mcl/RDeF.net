using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Mapping;
using RDeF.Mapping.Providers;

namespace Given_instance_of.DefaultMappingsRepository_class.which_is_already_initialized
{
    [TestFixture]
    public class and_searching_for_predicate_mapping : DefaultMappingsRepositoryTest
    {
        private const string ExpectedProperty = "Name";

        private IPropertyMapping Result { get; set; }

        public override void TheTest()
        {
            Result = MappingsRepository.FindPropertyMappingFor(null, new Iri(ExpectedProperty));
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
            var entityMapping = new Mock<IEntityMapping>(MockBehavior.Strict);
            entityMapping.SetupGet(instance => instance.Type).Returns(typeof(IProduct));
            var propertyMapping = new Mock<IPropertyMapping>(MockBehavior.Strict);
            propertyMapping.SetupGet(instance => instance.Name).Returns(ExpectedProperty);
            propertyMapping.SetupGet(instance => instance.Term).Returns(new Iri(ExpectedProperty));
            propertyMapping.SetupGet(instance => instance.Graph).Returns((Iri)null);
            entityMapping.SetupGet(instance => instance.Properties).Returns(new[] { propertyMapping.Object });
            MappingBuilder.Setup(instance => instance.BuildMappings(It.IsAny<IEnumerable<IMappingSource>>(), It.IsAny<IDictionary<Type, ICollection<ITermMappingProvider>>>()))
                .Returns(new Dictionary<Type, IEntityMapping>() { { typeof(IProduct), entityMapping.Object } });
        }
    }
}
