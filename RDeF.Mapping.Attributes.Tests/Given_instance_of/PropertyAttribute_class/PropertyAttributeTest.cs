using FluentAssertions;
using NUnit.Framework;
using PropertyAttribute = RDeF.Mapping.Attributes.PropertyAttribute;

namespace Given_instance_of.PropertyAttribute_class
{
    [TestFixture]
    public abstract class PropertyAttributeTest
    {
        protected const string ExpectedPrefix = "prefix";

        protected const string ExpectedTerm = "term";

        protected PropertyAttribute Attribute { get; private set; }

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
            Attribute = new PropertyAttribute(ExpectedPrefix, ExpectedTerm);
            ScenarioSetup();
            TheTest();
        }

        protected virtual void ScenarioSetup()
        {
        }
    }
}