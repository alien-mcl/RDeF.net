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
    public class DecimalConverter_class : LiteralConverterTest<DecimalConverter>
    {
        [TestCase("0", "en", 0d)]
        [TestCase("-8", "en", -8d)]
        [TestCase("2.12", "en", 2.12d)]
        [TestCase("-30.555", "en", -30.555d)]
        [TestCase("0", "pl", 0d)]
        [TestCase("-8", "pl", -8d)]
        [TestCase("2.12", "pl", 2.12d)]
        [TestCase("-30.555", "pl", -30.555d)]
        public void Should_convert_from_literal(string value, string culture, double expected)
        {
            Converter.Using(culture).ConvertFrom(StatementFor(value, xsd.@decimal)).Should().Be((decimal)expected);
        }

        [TestCase(0d, "en", "0")]
        [TestCase(-8d, "en", "-8")]
        [TestCase(2.12d, "en", "2.12")]
        [TestCase(-30.555d, "en", "-30.555")]
        [TestCase(0d, "pl", "0")]
        [TestCase(-8d, "pl", "-8")]
        [TestCase(2.12d, "pl", "2.12")]
        [TestCase(-30.555d, "pl", "-30.555")]
        public void Should_convert_to_literal(double value, string culture, string expectedLiteral)
        {
            Converter.Using(culture).ConvertTo(Subject, Predicate, (decimal)value).Should().MatchLiteralValueOf(expectedLiteral, xsd.@decimal);
        }

        [TestCase(xsd.ns + "decimal")]
        public void Should_enlist_supported_data_types(string dataType)
        {
            Converter.SupportedDataTypes.Should().Contain(new Iri(dataType));
        }

        [TestCase(typeof(decimal))]
        public void Should_enlist_supported_type(Type type)
        {
            Converter.SupportedTypes.Should().Contain(type);
        }

        [Test]
        public void Should_not_convert_scientific_notation()
        {
            Converter.Invoking(instance => instance.ConvertFrom(StatementFor("2e10", xsd.@decimal))).ShouldThrow<FormatException>();
        }
    }
}