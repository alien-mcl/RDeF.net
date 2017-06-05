using FluentAssertions;
using NUnit.Framework;
using RDeF.FluentAssertions;
using RDeF.Serialization;
using RDeF.Testing;

namespace Given_instance_of.JsonLdReader_class.when_deserializing
{
    [TestFixture]
    public class named_graphs : RdfReaderDeserializationTest<JsonLdReader>
    {
        [Test]
        public void Should_deserialize_from_Json_Ld_correctly()
        {
            Result.Should().MatchStatements(Expected);
        }

        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();
            WithSimpleGraph();
        }
    }
}
