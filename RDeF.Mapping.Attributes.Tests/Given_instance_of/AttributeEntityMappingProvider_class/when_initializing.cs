using FluentAssertions;
using NUnit.Framework;
using RDeF;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Mapping;
using RDeF.Mapping.Attributes;
using RDeF.Mapping.Providers;

namespace Given_instance_of.AttributeEntityMappingProvider_class
{
    [TestFixture]
    public class when_initializing : AttributePropertyMappingProviderTest
    {
        [Test]
        public void Should_create_an_instance_from_iri_attribute()
        {
            AttributeEntityMappingProvider.FromAttribute(EntityType, new ClassAttribute() { Iri = "test" })
                .Should().BeOfType<AttributeEntityMappingProvider>().Which.MatchesMapped<IProduct>(new Iri("test"));
        }

        [Test]
        public void Should_create_an_instance_from_iri_attribute_with_graph_iri()
        {
            AttributeEntityMappingProvider.FromAttribute(EntityType, new ClassAttribute() { Iri = "test", Graph = "graph" })
                .Should().BeOfType<AttributeEntityMappingProvider>().Which.MatchesMapped<IProduct>(new Iri("test"), new Iri("graph"));
        }

        [Test]
        public void Should_create_an_instance_from_qiri_attribute()
        {
            AttributeEntityMappingProvider.FromAttribute(EntityType, new ClassAttribute("test", "term"))
                .Should().BeOfType<AttributeEntityMappingProvider>()
                .Which.MatchesMapped<IProduct>(new QIriMapping("test", new Iri("test_")), new Iri("test_term"));
        }

        [Test]
        public void Should_create_an_instance_from_qiri_attribute_with_graph_iri()
        {
            AttributeEntityMappingProvider.FromAttribute(EntityType, new ClassAttribute("test", "term") { Graph = "graph" })
                .Should().BeOfType<AttributeEntityMappingProvider>()
                .Which.MatchesMapped<IProduct>(new QIriMapping("test", new Iri("test_")), new Iri("test_term"), new Iri("graph"));
        }

        [Test]
        public void Should_create_an_instance_from_qiri_attribute_with_graph_qiri()
        {
            AttributeEntityMappingProvider.FromAttribute(EntityType, new ClassAttribute("test", "term", "test", "graph"))
                .Should().BeOfType<AttributeEntityMappingProvider>()
                .Which.MatchesMapped<IProduct>(new QIriMapping("test", new Iri("test_")), new Iri("test_term"), new Iri("test_graph"));
        }
    }
}
