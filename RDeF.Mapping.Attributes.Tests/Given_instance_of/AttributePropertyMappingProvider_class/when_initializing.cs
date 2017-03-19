using FluentAssertions;
using NUnit.Framework;
using RDeF;
using RDeF.Entities;
using RDeF.Mapping;
using RDeF.Mapping.Providers;
using PropertyAttribute = RDeF.Mapping.Attributes.PropertyAttribute;

namespace Given_instance_of.AttributePropertyMappingProvider_class
{
    [TestFixture]
    public class when_initializing : AttributePropertyMappingProviderTest
    {
        [Test]
        public void Should_create_an_instance_from_iri_attribute()
        {
            AttributePropertyMappingProvider.FromAttribute(EntityType, Property, new PropertyAttribute() { Iri = "test", ValueConverterType = typeof(TestConverter) })
                .Should().BeOfType<AttributePropertyMappingProvider>().Which.MatchesMapped<TestConverter>(Property, new Iri("test"));
        }

        [Test]
        public void Should_create_an_instance_from_iri_attribute_with_graph_iri()
        {
            AttributePropertyMappingProvider.FromAttribute(EntityType, Property, new PropertyAttribute() { Iri = "test", Graph = "graph", ValueConverterType = typeof(TestConverter) })
                .Should().BeOfType<AttributePropertyMappingProvider>().Which.MatchesMapped<TestConverter>(Property, new Iri("test"), new Iri("graph"));
        }

        [Test]
        public void Should_create_an_instance_from_qiri_attribute()
        {
            AttributePropertyMappingProvider.FromAttribute(EntityType, Property, new PropertyAttribute("test", "term") { ValueConverterType = typeof(TestConverter) })
                .Should().BeOfType<AttributePropertyMappingProvider>()
                .Which.MatchesMapped<TestConverter>(Property, new QIriMapping("test", new Iri("test_")), new Iri("test_term"));
        }

        [Test]
        public void Should_create_an_instance_from_qiri_attribute_with_graph_iri()
        {
            AttributePropertyMappingProvider.FromAttribute(EntityType, Property, new PropertyAttribute("test", "term") { Graph = "graph", ValueConverterType = typeof(TestConverter) })
                .Should().BeOfType<AttributePropertyMappingProvider>()
                .Which.MatchesMapped<TestConverter>(Property, new QIriMapping("test", new Iri("test_")), new Iri("test_term"), new Iri("graph"));
        }

        [Test]
        public void Should_create_an_instance_from_qiri_attribute_with_graph_qiri()
        {
            AttributePropertyMappingProvider.FromAttribute(EntityType, Property, new PropertyAttribute("test", "term", "test", "graph") { ValueConverterType = typeof(TestConverter) })
                .Should().BeOfType<AttributePropertyMappingProvider>()
                .Which.MatchesMapped<TestConverter>(Property, new QIriMapping("test", new Iri("test_")), new Iri("test_term"), new Iri("test_graph"));
        }
    }
}
