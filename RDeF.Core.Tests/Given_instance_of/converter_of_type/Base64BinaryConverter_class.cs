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
    public class Base64BinaryConverter_class : LiteralConverterTest<Base64BinaryConverter>
    {
        [TestCase(new byte[] { 0, 255 })]
        public void Should_convert_from_literal(byte[] value)
        {
            Converter.ConvertFrom(StatementFor(Convert.ToBase64String(value), xsd.base64Binary)).Should().BeOfType<byte[]>().Which.ShouldBeEquivalentTo(value);
        }

        [TestCase(new byte[] { 0, 255 })]
        public void Should_convert_to_literal(byte[] value)
        {
            Converter.ConvertTo(Subject, Predicate, value).Should().MatchLiteralValueOf(Convert.ToBase64String(value), xsd.base64Binary);
        }

        [TestCase(xsd.ns + "base64Binary")]
        public void Should_enlist_supported_data_types(string dataType)
        {
            Converter.SupportedDataTypes.Should().Contain(new Iri(dataType));
        }

        [TestCase(typeof(byte[]))]
        public void Should_enlist_supported_type(Type type)
        {
            Converter.SupportedTypes.Should().Contain(type);
        }
    }
}