using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using RDeF.FluentAssertions;
using RDeF.Serialization;
using RDeF.Testing;

namespace Given_instance_of.RdfXmlWriter_class.when_serializing
{
    [TestFixture]
    public class named_graphs : RdfWriterSerializationTest<RdfXmlWriter>
    {
        [Test]
        public async Task Should_serialize_to_Rdf_Xml_correctly()
        {
            (await new RdfXmlReader().Read(new StreamReader(Stream))).Should().MatchStatementsInAnyGraph(Graphs);
        }

        protected override void ScenarioSetup()
        {
            WithSimpleGraph();
            base.ScenarioSetup();
        }
    }
}
