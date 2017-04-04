using System.Reflection;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Mapping;
using RDeF.Mapping.Explicit;

namespace Given_instance_of.DefaultExplicitMappings_class
{
    [TestFixture]
    public class when_obtaining_mappings
    {
        private Mock<IPropertyMapping> PropertyMapping { get; set; }

        private Mock<IEntityMapping> EntityMapping { get; set; }

        private DefaultExplicitMappings Mappings { get; set; }

        [Test]
        public void Should_retrieve_entity_mapping()
        {
            Mappings.FindEntityMappingFor(typeof(IProduct)).Should().Be(EntityMapping.Object);
        }

        [Test]
        public void Should_retrieve_property_mapping()
        {
            Mappings.FindPropertyMappingFor(typeof(IProduct).GetProperty("Description")).Should().Be(PropertyMapping.Object);
        }

        [SetUp]
        public void Setup()
        {
            PropertyMapping = new Mock<IPropertyMapping>(MockBehavior.Strict);
            PropertyMapping.SetupGet(instance => instance.Name).Returns("Description");
            EntityMapping = new Mock<IEntityMapping>(MockBehavior.Strict);
            EntityMapping.SetupGet(instance => instance.Type).Returns(typeof(IProduct));
            EntityMapping.SetupGet(instance => instance.Properties).Returns(new[] { PropertyMapping.Object });
            Mappings = new DefaultExplicitMappings();
            Mappings.Set(EntityMapping.Object);
        }
    }
}
