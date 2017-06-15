using FluentAssertions;
using NUnit.Framework;
using RDeF.FluentAssertions;
using RDeF.Serialization;
using RDeF.Testing;

namespace Given_instance_of.RdfXmlReader_class.when_deserializing
{
    [TestFixture]
    public class named_graphs_with_lists_and_collections : RdfReaderDeserializationTest<RdfXmlReader>
    {
        [Test]
        public void Should_deserialize_to_Rdf_Xml_correctly()
        {
            Result.Should().BeSimilarToInAnyGraph(Expected);
        }

        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();
            WithCollectionsGraph();
        }
    }
}
