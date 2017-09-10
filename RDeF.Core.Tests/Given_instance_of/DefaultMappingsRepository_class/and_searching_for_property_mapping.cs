using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Mapping;
using RDeF.Mapping.Providers;

namespace Given_instance_of.DefaultMappingsRepository_class.which_is_already_initialized
{
    [TestFixture]
    public class and_searching_for_property_mapping : DefaultMappingsRepositoryTest
    {
        private static readonly PropertyInfo ExpectedProperty = typeof(IProduct).GetProperty("Name");

        private IPropertyMapping Result { get; set; }

        public override void TheTest()
        {
            Result = MappingsRepository.FindPropertyMappingFor(ExpectedProperty);
        }

        [Test]
        public void Should_retrieve_the_property_mapping()
        {
            Result.Name.Should().Be(ExpectedProperty.Name);
        }

        [Test]
        public void Should_throw_when_no_property_is_given()
        {
            MappingsRepository.Invoking(instance => instance.FindPropertyMappingFor(null)).ShouldThrow<ArgumentNullException>();
        }

        protected override void ScenarioSetup()
        {
            var entityMapping = new Mock<IEntityMapping>(MockBehavior.Strict);
            entityMapping.SetupGet(instance => instance.Type).Returns(typeof(IProduct));
            var propertyMapping = new Mock<IPropertyMapping>(MockBehavior.Strict);
            propertyMapping.SetupGet(instance => instance.Name).Returns(ExpectedProperty.Name);
            propertyMapping.SetupGet(instance => instance.EntityMapping).Returns(entityMapping.Object);
            entityMapping.SetupGet(instance => instance.Properties).Returns(new[] { propertyMapping.Object });
            MappingBuilder.Setup(instance => instance.BuildMappings(It.IsAny<IEnumerable<IMappingSource>>(), It.IsAny<IDictionary<Type, ICollection<ITermMappingProvider>>>()))
                .Returns(new Dictionary<Type, IEntityMapping>() { { typeof(IProduct), entityMapping.Object } });
        }
    }
}
