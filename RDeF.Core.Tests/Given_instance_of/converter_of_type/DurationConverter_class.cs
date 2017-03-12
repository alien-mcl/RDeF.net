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
    public class DurationConverter_class : LiteralConverterTest<DurationConverter>
    {
        [TestCase("PT1H30M", 1, 30, 0, 0)]
        [TestCase("-PT1H30M", -1, 30, 0, 0)]
        public void Should_convert_from_literal(string value, int hours, int minutes, int seconds, int milliseconds)
        {
            Converter.ConvertFrom(StatementFor(value, xsd.duration)).Should().Be(new Duration(hours < 0, Math.Abs(hours), minutes, seconds, milliseconds));
        }

        [TestCase(1, 30, 0, 0, "PT1H30M")]
        [TestCase(-1, 30, 0, 0, "-PT1H30M")]
        public void Should_convert_to_literal(int hours, int minutes, int seconds, int milliseconds, string expectedLiteral)
        {
            Converter.ConvertTo(Subject, Predicate, new Duration(hours < 0, Math.Abs(hours), minutes, seconds, milliseconds))
                .Should().MatchLiteralValueOf(expectedLiteral, xsd.duration);
        }

        [TestCase(xsd.ns + "duration")]
        public void Should_enlist_supported_data_types(string dataType)
        {
            Converter.SupportedDataTypes.Should().Contain(new Iri(dataType));
        }

        [TestCase(typeof(Duration))]
        public void Should_enlist_supported_type(Type type)
        {
            Converter.SupportedTypes.Should().Contain(type);
        }
    }
}