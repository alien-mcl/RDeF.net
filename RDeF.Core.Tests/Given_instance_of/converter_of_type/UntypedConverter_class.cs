using System;
using System.ComponentModel;
using FluentAssertions;
using NUnit.Framework;
using RDeF;
using RDeF.Mapping.Converters;

namespace Given_instance_of.converter_of_type
{
    [TestFixture]
    public class UntypedConverter_class : LiteralConverterTest<UntypedLiteralConverter>
    {
        [TestCase("http://test.com/", typeof(string), "http://test.com/")]
        [TestCase("6cbb1450-c74d-47e8-a1e0-50f1db6cfd6f", typeof(Guid), "6cbb1450-c74d-47e8-a1e0-50f1db6cfd6f")]
        [TestCase("{6cbb1450-c74d-47e8-a1e0-50f1db6cfd6f}", typeof(Guid), "6cbb1450-c74d-47e8-a1e0-50f1db6cfd6f")]
        [TestCase("(6cbb1450-c74d-47e8-a1e0-50f1db6cfd6f)", typeof(Guid), "6cbb1450-c74d-47e8-a1e0-50f1db6cfd6f")]
        public void Should_convert_from_literal(string value, Type type, string expected)
        {
            Converter.ConvertFrom(StatementFor(value)).Should().Be(TypeDescriptor.GetConverter(type).ConvertFrom(expected));
        }

        [TestCase("http://test.com/", typeof(string), "http://test.com/")]
        [TestCase("6cbb1450-c74d-47e8-a1e0-50f1db6cfd6f", typeof(Guid), "{6cbb1450-c74d-47e8-a1e0-50f1db6cfd6f}")]
        public void Should_convert_to_literal(string value, Type type, string expectedLiteral)
        {
            Converter.ConvertTo(Subject, Predicate, TypeDescriptor.GetConverter(type).ConvertFrom(value)).Should().MatchLiteralValueOf(expectedLiteral);
        }

        [Test]
        public void Should_not_enlist_any_supported_data_types()
        {
            Converter.SupportedDataTypes.Should().BeEmpty();
        }

        [Test]
        public void Should_not_enlist_any_supported_type()
        {
            Converter.SupportedTypes.Should().BeEmpty();
        }
    }
}