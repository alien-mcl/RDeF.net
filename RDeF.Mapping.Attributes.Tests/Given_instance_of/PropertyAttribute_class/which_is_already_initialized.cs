using FluentAssertions;
using NUnit.Framework;
using PropertyAttribute = RDeF.Mapping.Attributes.PropertyAttribute;

namespace Given_instance_of.PropertyAttribute_class
{
    [TestFixture]
    public class which_is_already_initialized : PropertyAttributeTest
    {
        private const string ExpectedIri = "some:iri";

        private const string ExpectedGraphIri = "some:iri";

        [Test]
        public void Should_obtain_mapped_term_iri()
        {
            Attribute.Iri.Should().Be(ExpectedIri);
        }

        [Test]
        public void Should_obtain_mapped_graph_iri()
        {
            Attribute.Graph.Should().Be(ExpectedGraphIri);
        }

        [Test]
        public void Should_obtain_a_value_converter_type()
        {
            Attribute.ValueConverterType.Should().BeNull();
        }

        protected override void ScenarioSetup()
        {
            Attribute = new PropertyAttribute() { Iri = ExpectedIri, Graph = ExpectedGraphIri };
        }
    }
}
