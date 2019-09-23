using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using RDeF.FluentAssertions;
using RDeF.Serialization;
using RDeF.Testing;

namespace Given_instance_of.JsonLdWriter_class.when_serializing
{
    [TestFixture]
    public class named_graphs_with_lists_and_collections : RdfWriterSerializationTest<JsonLdWriter>
    {
        [Test]
        public async Task Should_serialize_to_Json_Ld_correctly()
        {
            (await new JsonLdReader().Read(new StreamReader(Stream))).Should().BeSimilarTo(Graphs);
        }

        protected override void ScenarioSetup()
        {
            WithCollectionsGraph();
            base.ScenarioSetup();
        }
    }
}
