using FluentAssertions;
using NUnit.Framework;
using RDeF.FluentAssertions;
using RDeF.Serialization;
using RDeF.Testing;

namespace Given_instance_of.JsonLdReader_class.when_deserializing
{
    [TestFixture]
    public class named_graphs_with_lists_and_collections : RdfReaderDeserializationTest<JsonLdReader>
    {
        [Test]
        public void Should_deserialize_to_Json_Ld_correctly()
        {
            Result.Should().BeSimilarTo(Expected);
        }

        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();
            WithCollectionsGraph();
        }
    }
}
