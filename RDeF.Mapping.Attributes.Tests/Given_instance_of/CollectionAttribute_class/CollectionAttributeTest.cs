using FluentAssertions;
using NUnit.Framework;
using RDeF.Mapping.Attributes;

namespace Given_instance_of.CollectionAttribute_class
{
    [TestFixture]
    public abstract class CollectionAttributeTest
    {
        protected const string ExpectedPrefix = "prefix";

        protected const string ExpectedTerm = "term";

        protected CollectionAttribute Attribute { get; private set; }

        public virtual void TheTest()
        {
        }

        [Test]
        public void Should_obtain_a_value_converter_type()
        {
            Attribute.ValueConverterType.Should().BeNull();
        }

        [SetUp]
        public void Setup()
        {
            Attribute = new CollectionAttribute(ExpectedPrefix, ExpectedTerm);
            ScenarioSetup();
            TheTest();
        }

        protected virtual void ScenarioSetup()
        {
        }
    }
}