using System;
using System.Reflection;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Mapping;
using RollerCaster;

namespace Given_instance_of.DefaultEntityContext_class
{
    [TestFixture]
    public abstract class ScenarioTest : DefaultEntityContextTest
    {
        private const string ExpectedName = "Product name";
        private static readonly Iri PredicateIri = new Iri("name");
        private static readonly Iri Iri = new Iri(new Uri("http://test.com/"));

        protected Mock<IPropertyMapping> PropertyMapping { get; private set; }

        private Entity Entity { get; set; }

        private Mock<IEntityMapping> EntityMapping { get; set; }

        private Mock<IConverter> Converter { get; set; }

        public override void TheTest()
        {
            Context.Initialize(Entity);
        }

        [Test]
        public void Should_load_statements_from_the_entity_source()
        {
            EntitySource.Verify(instance => instance.Load(Iri), Times.Once);
        }

        [Test]
        public void Should_search_for_mappings_for_a_statement()
        {
            MappingsRepository.Verify(instance => instance.FindPropertyMappingFor(PredicateIri, null), Times.Once);
        }

        [Test]
        public void Should_use_mappings_graph_for_matching()
        {
            PropertyMapping.VerifyGet(instance => instance.Graph, Times.Once);
        }

        [Test]
        public void Should_obtain_a_value_converter_for_a_statement()
        {
            PropertyMapping.VerifyGet(instance => instance.ValueConverter, Times.Once);
        }

        [Test]
        public void Should_use_the_converter_to_convert_a_statements_value()
        {
            Converter.Verify(instance => instance.ConvertFrom(It.Is<Statement>(statement => statement.Subject == Iri && statement.Value == ExpectedName)), Times.Once);
        }

        [Test]
        public void Should_set_the_statements_value_in_the_proxy()
        {
            Entity.ActLike<IProduct>().Name.Should().Be(ExpectedName);
        }

        protected override void ScenarioSetup()
        {
            Entity = new Entity(Iri, Context);
            EntitySource.Setup(instance => instance.Load(It.IsAny<Iri>()))
                .Returns<Iri>(iri => new[] { new Statement(iri, PredicateIri, ExpectedName) });
            EntityMapping = new Mock<IEntityMapping>(MockBehavior.Strict);
            EntityMapping.SetupGet(instance => instance.Type).Returns(typeof(IProduct));
            Converter = new Mock<IConverter>(MockBehavior.Strict);
            Converter.Setup(instance => instance.ConvertFrom(It.IsAny<Statement>()))
                .Returns<Statement>(statement => statement.Value);
            var property = new Mock<PropertyInfo>(MockBehavior.Strict);
            property.SetupGet(instance => instance.Name).Returns("Name");
            property.SetupGet(instance => instance.PropertyType).Returns(typeof(string));
            property.SetupGet(instance => instance.DeclaringType).Returns(typeof(IProduct));
            property.Setup(instance => instance.GetHashCode()).Returns(typeof(IProduct).GetProperty("Name").GetHashCode());
            property.Setup(instance => instance.Equals(It.IsAny<object>())).Returns<object>(obj => (obj as PropertyInfo)?.Name == "Name");
            PropertyMapping = new Mock<IPropertyMapping>(MockBehavior.Strict);
            PropertyMapping.SetupGet(instance => instance.EntityMapping).Returns(EntityMapping.Object);
            PropertyMapping.SetupGet(instance => instance.PropertyInfo).Returns(property.Object);
            PropertyMapping.SetupGet(instance => instance.Name).Returns("Name");
            PropertyMapping.SetupGet(instance => instance.Graph).Returns((Iri)null);
            PropertyMapping.SetupGet(instance => instance.ValueConverter).Returns(Converter.Object);
        }
    }
}
