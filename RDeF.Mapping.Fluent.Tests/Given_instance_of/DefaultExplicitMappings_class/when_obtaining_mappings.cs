using System;
using System.Reflection;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
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

        private Iri OwningEntity { get; set; }

        public void TheTest()
        {
            Mappings.Set(EntityMapping.Object, OwningEntity);
        }

        [Test]
        public void Should_retrieve_entity_mapping()
        {
            Mappings.FindEntityMappingFor(typeof(IProduct), OwningEntity).Type.Should().Be<IProduct>();
        }

        [Test]
        public void Should_retrieve_property_mapping()
        {
            Mappings.FindPropertyMappingFor(typeof(IProduct).GetTypeInfo().GetProperty("Description"), OwningEntity).Should().Be(PropertyMapping.Object);
        }

        [SetUp]
        public void Setup()
        {
            PropertyMapping = new Mock<IPropertyMapping>(MockBehavior.Strict);
            PropertyMapping.SetupGet(instance => instance.Name).Returns("Description");
            PropertyMapping.SetupGet(instance => instance.Term).Returns(new Iri("description"));
            PropertyMapping.SetupGet(instance => instance.Graph).Returns((Iri)null);
            PropertyMapping.SetupGet(instance => instance.ValueConverter).Returns((ILiteralConverter)null);
            PropertyMapping.SetupGet(instance => instance.PropertyInfo).Returns(typeof(IProduct).GetTypeInfo().GetProperty("Description"));
            EntityMapping = new Mock<IEntityMapping>(MockBehavior.Strict);
            EntityMapping.SetupGet(instance => instance.Type).Returns(typeof(IProduct));
            EntityMapping.SetupGet(instance => instance.Classes).Returns(Array.Empty<IStatementMapping>());
            EntityMapping.SetupGet(instance => instance.Properties).Returns(new[] { PropertyMapping.Object });
            OwningEntity = new Iri("http://temp.uri/");
            Mappings = new DefaultExplicitMappings();
            TheTest();
        }
    }
}
