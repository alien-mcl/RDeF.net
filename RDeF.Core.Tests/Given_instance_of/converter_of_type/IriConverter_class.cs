using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Entities;
using RDeF.Mapping.Converters;

namespace Given_instance_of.converter_of_type
{
    [TestFixture]
    public class IriConverter_class : LiteralConverterTest<IriConverter>
    {
        [TestCase("urn:some-name")]
        [TestCase("http://temp.uri/")]
        [TestCase("http://temp.uri/document#fragment")]
        public void Should_convert_from_literal(string value)
        {
            Converter.ConvertFrom(new Statement(Subject, Predicate, new Iri(value))).Should().Be(new Iri(value));
        }

        [TestCase("urn:some-name")]
        [TestCase("http://temp.uri/")]
        [TestCase("http://temp.uri/document#fragment")]
        public void Should_convert_to_literal(string value)
        {
            Converter.ConvertTo(Subject, Predicate, new Iri(value)).Object.Should().Be(new Iri(value));
        }

        [Test]
        public void Should_enlist_supported_data_types()
        {
            Converter.SupportedDataTypes.Should().BeEmpty();
        }

        [Test]
        public void Should_not_enlist_any_supported_type()
        {
            Converter.SupportedTypes.Should().HaveCount(1).And.Subject.First().Should().Be(typeof(Iri));
        }
    }
}