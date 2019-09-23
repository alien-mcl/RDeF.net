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
    public class named_graphs : RdfWriterSerializationTest<JsonLdWriter>
    {
        [Test]
        public async Task Should_serialize_to_Json_Ld_correctly()
        {
            (await new JsonLdReader().Read(new StreamReader(Stream))).Should().MatchStatements(Graphs);
        }

        protected override void ScenarioSetup()
        {
            WithSimpleGraph();
            base.ScenarioSetup();
        }
    }
}
