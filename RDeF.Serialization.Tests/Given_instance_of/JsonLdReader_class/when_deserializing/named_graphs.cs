using FluentAssertions;
using NUnit.Framework;
using RDeF.FluentAssertions;

namespace Given_instance_of.JsonLdReader_class.when_deserializing
{
    [TestFixture]
    public class named_graphs : ScenarioTest
    {
        [Test]
        public void Should_deserialize_from_Json_Ld_correctly()
        {
            Result.Should().MatchStatements(Expected);
        }

        protected override void ScenarioSetup()
        {
            WithSimpleGraph();
            base.ScenarioSetup();
        }
    }
}
