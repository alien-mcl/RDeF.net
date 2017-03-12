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
    public class StringConverter_class : LiteralConverterTest<StringConverter>
    {
        [TestCase("test")]
        public void Should_convert_from_literal(string value)
        {
            Converter.ConvertFrom(StatementFor(value, xsd.@string)).Should().Be(value);
        }

        [TestCase("test")]
        public void Should_convert_to_literal(string value)
        {
            Converter.ConvertTo(Subject, Predicate, value).Should().MatchLiteralValueOf(value, xsd.@string);
        }

        [TestCase(xsd.ns + "string")]
        public void Should_enlist_supported_data_types(string dataType)
        {
            Converter.SupportedDataTypes.Should().Contain(new Iri(dataType));
        }

        [TestCase(typeof(string))]
        public void Should_enlist_supported_type(Type type)
        {
            Converter.SupportedTypes.Should().Contain(type);
        }
    }
}