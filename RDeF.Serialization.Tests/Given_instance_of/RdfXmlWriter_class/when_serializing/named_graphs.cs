using System.Text;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Serialization;
using RDeF.Testing;

namespace Given_instance_of.RdfXmlWriter_class.when_serializing
{
    [TestFixture]
    public class named_graphs : RdfWriterSerializationTest<RdfXmlWriter>
    {
        [Test]
        public void Should_serialize_to_Rdf_Xml_correctly()
        {
            Encoding.UTF8.GetString(Stream.ToArray()).Should().Be(Expected);
        }

        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();
            WithSimpleGraph();
        }
    }
}
