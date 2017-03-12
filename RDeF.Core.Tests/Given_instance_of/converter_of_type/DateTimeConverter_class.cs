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
    public class DateTimeConverter_class : LiteralConverterTest<DateTimeConverter>
    {
        [TestCase("2010-03-23", xsd.ns + "date", 2010, 03, 23, 00, 00, 00, 0)]
        [TestCase("14:30:44", xsd.ns + "time", 0001, 01, 01, 14, 30, 44, 00)]
        [TestCase("2010-03-23T12:30:44", xsd.ns + "dateTime", 2010, 03, 23, 12, 30, 44, 0)]
        [TestCase("2010-03-23T14:30:44+05:30", xsd.ns + "dateTime", 2010, 03, 23, 09, 00, 44, 0)]
        [TestCase("2010-03-23T14:30:44Z", xsd.ns + "dateTime", 2010, 03, 23, 14, 30, 44, 0)]
        [TestCase("2010-03-23T14:30:44+14:00", xsd.ns + "dateTime", 2010, 03, 23, 00, 30, 44, 0)]
        [TestCase("14:30:44+05:30", xsd.ns + "time", 0001, 01, 01, 09, 00, 44, 0)]
        [TestCase("14:30:44Z", xsd.ns + "time", 0001, 01, 01, 14, 30, 44, 0)]
        [TestCase("14:30:44+14:00", xsd.ns + "time", 0001, 01, 01, 00, 30, 44, 0)]
        public void Should_convert_from_literal(string value, string dataType, int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            ((DateTime)Converter.ConvertFrom(StatementFor(value, new Iri(dataType)))).Should().Be(new DateTime(year, month, day, hour, minute, second, millisecond));
        }

        [TestCase(2010, 03, 23, 00, 00, 00, 0, "2010-03-23", xsd.ns + "date")]
        [TestCase(0001, 01, 01, 14, 30, 44, 00, "14:30:44", xsd.ns + "time")]
        [TestCase(2010, 03, 23, 12, 30, 44, 0, "2010-03-23T12:30:44", xsd.ns + "dateTime")]
        public void Should_convert_to_literal(int year, int month, int day, int hour, int minute, int second, int millisecond, string value, string dataType)
        {
            Converter.ConvertTo(Subject, Predicate, new DateTime(year, month, day, hour, minute, second, millisecond)).Should().MatchLiteralValueOf(value, new Iri(dataType));
        }

        [TestCase(xsd.ns + "date")]
        [TestCase(xsd.ns + "dateTime")]
        [TestCase(xsd.ns + "time")]
        public void Should_enlist_supported_data_types(string dataType)
        {
            Converter.SupportedDataTypes.Should().Contain(new Iri(dataType));
        }

        [TestCase(typeof(DateTime))]
        public void Should_enlist_supported_type(Type type)
        {
            Converter.SupportedTypes.Should().Contain(type);
        }
    }
}