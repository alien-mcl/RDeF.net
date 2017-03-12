using System;
using FluentAssertions;
using NUnit.Framework;
using RDeF;
using RDeF.Entities;
using RDeF.Mapping.Converters;
using RDeF.Vocabularies;

namespace Given_instance_of.converter_of_type
{
    [TestFixture]
    public class UriConverter_class : LiteralConverterTest<UriConverter>
    {
        [TestCase("http://test.com/")]
        [TestCase("urn:uuid:6cbb1450-c74d-47e8-a1e0-50f1db6cfd6f")]
        public void Should_convert_from_literal(string value)
        {
            Converter.ConvertFrom(StatementFor(value, xsd.anyUri)).Should().Be(new Uri(value));
        }

        [TestCase("http://test.com/")]
        [TestCase("urn:uuid:6cbb1450-c74d-47e8-a1e0-50f1db6cfd6f")]
        public void Should_convert_to_literal(string value)
        {
            Converter.ConvertTo(Subject, Predicate, new Uri(value)).Should().MatchLiteralValueOf(value, xsd.anyUri);
        }

        [TestCase(xsd.ns + "anyUri")]
        public void Should_enlist_supported_data_types(string dataType)
        {
            Converter.SupportedDataTypes.Should().Contain(new Iri(dataType));
        }

        [TestCase(typeof(Uri))]
        public void Should_enlist_supported_type(Type type)
        {
            Converter.SupportedTypes.Should().Contain(type);
        }
    }
}