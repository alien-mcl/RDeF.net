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
    public class BooleanConverter_class : LiteralConverterTest<BooleanConverter>
    {
        [TestCase("true", true)]
        [TestCase("false", false)]
        public void Should_convert_from_literal(string value, bool expected)
        {
            Converter.ConvertFrom(StatementFor(value, xsd.boolean)).Should().Be(expected);
        }

        [TestCase(true, "true")]
        [TestCase(false, "false")]
        public void Should_convert_to_literal(bool value, string expectedLiteral)
        {
            Converter.ConvertTo(Subject, Predicate, value).Should().MatchLiteralValueOf(expectedLiteral, xsd.boolean);
        }

        [TestCase(xsd.ns + "boolean")]
        public void Should_enlist_supported_data_types(string dataType)
        {
            Converter.SupportedDataTypes.Should().Contain(new Iri(dataType));
        }

        [TestCase(typeof(bool))]
        public void Should_enlist_supported_type(Type type)
        {
            Converter.SupportedTypes.Should().Contain(type);
        }
    }
}