using FluentAssertions;
using NUnit.Framework;
using RDeF.FluentAssertions;

namespace Given_instance_of.RdfXmlReader_class.when_deserializing
{
    [TestFixture]
    public class named_graphs : ScenarioTest
    {
        [Test]
        public void Should_deserialize_from_Rdf_Xml_correctly()
        {
            Result.Should().MatchStatementsInAnyGraph(Expected);
        }

        protected override void ScenarioSetup()
        {
            WithSimpleGraph();
            base.ScenarioSetup();
        }
    }
}
