using FluentAssertions;
using NUnit.Framework;
using RDeF;
using RDeF.Entities;
using RDeF.Mapping;
using RDeF.Mapping.Attributes;
using RDeF.Mapping.Providers;

namespace Given_instance_of.AttributeCollectionMappingProvider_class
{
    [TestFixture]
    public class when_initializing : AttributeCollectionMappingProviderTest
    {
        [Test]
        public void Should_create_an_instance_from_iri_attribute()
        {
            AttributeCollectionMappingProvider.FromAttribute(EntityType, Property, AttributeMadeFrom<TestConverter>("test"))
                .Should().BeOfType<AttributeCollectionMappingProvider>().Which.MatchesMapped<TestConverter>(Property, new Iri("test"));
        }

        [Test]
        public void Should_create_an_instance_from_iri_attribute_with_graph_iri()
        {
            AttributeCollectionMappingProvider.FromAttribute(EntityType, Property, AttributeMadeFrom<TestConverter>("test", null, "graph"))
                .Should().BeOfType<AttributeCollectionMappingProvider>().Which.MatchesMapped<TestConverter>(Property, new Iri("test"), new Iri("graph"));
        }

        [Test]
        public void Should_create_an_instance_from_qiri_attribute()
        {
            AttributeCollectionMappingProvider.FromAttribute(EntityType, Property, AttributeMadeFrom<TestConverter>("term", "test"))
                .Should().BeOfType<AttributeCollectionMappingProvider>()
                .Which.MatchesMapped<TestConverter>(Property, new QIriMapping("test", new Iri("test_")), new Iri("test_term"));
        }

        [Test]
        public void Should_create_an_instance_from_qiri_attribute_with_graph_iri()
        {
            AttributeCollectionMappingProvider.FromAttribute(EntityType, Property, AttributeMadeFrom<TestConverter>("term", "test", "graph"))
                .Should().BeOfType<AttributeCollectionMappingProvider>()
                .Which.MatchesMapped<TestConverter>(Property, new QIriMapping("test", new Iri("test_")), new Iri("test_term"), new Iri("graph"));
        }

        [Test]
        public void Should_create_an_instance_from_qiri_attribute_with_graph_qiri()
        {
            AttributeCollectionMappingProvider.FromAttribute(EntityType, Property, AttributeMadeFrom<TestConverter>("term", "test", "graph", "test"))
                .Should().BeOfType<AttributeCollectionMappingProvider>()
                .Which.MatchesMapped<TestConverter>(Property, new QIriMapping("test", new Iri("test_")), new Iri("test_term"), new Iri("test_graph"));
        }

        private CollectionAttribute AttributeMadeFrom<TConverter>(string iriOrTerm, string prefix = null, string graph = null, string graphPrefix = null)
        {
            CollectionAttribute result;
            if (prefix == null)
            {
                result = new CollectionAttribute() { Iri = iriOrTerm, Graph = graph };
            }
            else
            {
                result = (graphPrefix == null ? new CollectionAttribute(prefix, iriOrTerm) { Graph = graph } : new CollectionAttribute(prefix, iriOrTerm, graphPrefix, graph));
            }

            result.ValueConverterType = typeof(TConverter);
            result.StoreAs = CollectionStorageModel.Simple;
            return result;
        }
    }
}
