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
    public class FloatingPointLiteralConverter_class : LiteralConverterTest<FloatingPointLiteralConverter>
    {
        [TestCase("+INF", xsd.ns + "double", "en", Double.PositiveInfinity)]
        [TestCase("INF", xsd.ns + "double", "en", Double.PositiveInfinity)]
        [TestCase("-INF", xsd.ns + "double", "en", Double.NegativeInfinity)]
        [TestCase("NaN", xsd.ns + "double", "en", Double.NaN)]
        [TestCase("0", xsd.ns + "double", "en", 0d)]
        [TestCase("2.12", xsd.ns + "double", "en", 2.12d)]
        [TestCase("2e10", xsd.ns + "double", "en", 2e10d)]
        [TestCase("+INF", xsd.ns + "float", "en", Single.PositiveInfinity)]
        [TestCase("INF", xsd.ns + "float", "en", Single.PositiveInfinity)]
        [TestCase("-INF", xsd.ns + "float", "en", Single.NegativeInfinity)]
        [TestCase("NaN", xsd.ns + "float", "en", Single.NaN)]
        [TestCase("0", xsd.ns + "float", "en", 0f)]
        [TestCase("2.12", xsd.ns + "float", "en", 2.12f)]
        [TestCase("2e10", xsd.ns + "float", "en", 2e10f)]
        [TestCase("+INF", xsd.ns + "double", "pl", Double.PositiveInfinity)]
        [TestCase("INF", xsd.ns + "double", "pl", Double.PositiveInfinity)]
        [TestCase("-INF", xsd.ns + "double", "pl", Double.NegativeInfinity)]
        [TestCase("NaN", xsd.ns + "double", "pl", Double.NaN)]
        [TestCase("0", xsd.ns + "double", "pl", 0d)]
        [TestCase("2.12", xsd.ns + "double", "pl", 2.12d)]
        [TestCase("2e10", xsd.ns + "double", "pl", 2e10d)]
        [TestCase("+INF", xsd.ns + "float", "pl", Single.PositiveInfinity)]
        [TestCase("INF", xsd.ns + "float", "pl", Single.PositiveInfinity)]
        [TestCase("-INF", xsd.ns + "float", "pl", Single.NegativeInfinity)]
        [TestCase("NaN", xsd.ns + "float", "pl", Single.NaN)]
        [TestCase("0", xsd.ns + "float", "pl", 0f)]
        [TestCase("2.12", xsd.ns + "float", "pl", 2.12f)]
        [TestCase("2e10", xsd.ns + "float", "pl", 2e10f)]
        public void Should_convert_from_literal(string value, string dataType, string culture, object expected)
        {
            Converter.Using(culture).ConvertFrom(StatementFor(value, new Iri(dataType))).Should().Be(expected);
        }

        [TestCase(Double.PositiveInfinity, "en", "INF", xsd.ns + "double")]
        [TestCase(Double.NegativeInfinity, "en", "-INF", xsd.ns + "double")]
        [TestCase(Double.NaN, "en", "NaN", xsd.ns + "double")]
        [TestCase(0d, "en", "0", xsd.ns + "double")]
        [TestCase(2.12d, "en", "2.12", xsd.ns + "double")]
        [TestCase(2e10d, "en", "20000000000", xsd.ns + "double")]
        [TestCase(Single.PositiveInfinity, "en", "INF", xsd.ns + "float")]
        [TestCase(Single.NegativeInfinity, "en", "-INF", xsd.ns + "float")]
        [TestCase(Single.NaN, "en", "NaN", xsd.ns + "float")]
        [TestCase(0f, "en", "0", xsd.ns + "float")]
        [TestCase(2.12f, "en", "2.12", xsd.ns + "float")]
        [TestCase(2e10f, "en", "2E+10", xsd.ns + "float")]
        [TestCase(Double.PositiveInfinity, "pl", "INF", xsd.ns + "double")]
        [TestCase(Double.NegativeInfinity, "pl", "-INF", xsd.ns + "double")]
        [TestCase(Double.NaN, "pl", "NaN", xsd.ns + "double")]
        [TestCase(0d, "pl", "0", xsd.ns + "double")]
        [TestCase(2.12d, "pl", "2.12", xsd.ns + "double")]
        [TestCase(2e10d, "pl", "20000000000", xsd.ns + "double")]
        [TestCase(Single.PositiveInfinity, "pl", "INF", xsd.ns + "float")]
        [TestCase(Single.NegativeInfinity, "pl", "-INF", xsd.ns + "float")]
        [TestCase(Single.NaN, "pl", "NaN", xsd.ns + "float")]
        [TestCase(0f, "pl", "0", xsd.ns + "float")]
        [TestCase(2.12f, "pl", "2.12", xsd.ns + "float")]
        [TestCase(2e10f, "pl", "2E+10", xsd.ns + "float")]
        public void Should_convert_to_literal(object value, string culture, string expectedLiteral, string dataType)
        {
            Converter.Using(culture).ConvertTo(Subject, Predicate, value).Should().MatchLiteralValueOf(expectedLiteral, new Iri(dataType));
        }

        [TestCase(xsd.ns + "double")]
        [TestCase(xsd.ns + "float")]
        public void Should_enlist_supported_data_types(string dataType)
        {
            Converter.SupportedDataTypes.Should().Contain(new Iri(dataType));
        }

        [TestCase(typeof(double))]
        [TestCase(typeof(float))]
        public void Should_enlist_supported_type(Type type)
        {
            Converter.SupportedTypes.Should().Contain(type);
        }
    }
}