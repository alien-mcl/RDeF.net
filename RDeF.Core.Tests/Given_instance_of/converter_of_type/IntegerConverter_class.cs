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
    public class IntegerConverter_class : LiteralConverterTest<IntegerConverter>
    {
        [TestCase("0", xsd.ns + "byte", (sbyte)0)]
        [TestCase("5", xsd.ns + "byte", (sbyte)5)]
        [TestCase("-20", xsd.ns + "byte", (sbyte)-20)]
        [TestCase("0", xsd.ns + "unsignedByte", (byte)0)]
        [TestCase("5", xsd.ns + "unsignedByte", (byte)5)]
        [TestCase("0", xsd.ns + "short", (short)0)]
        [TestCase("5", xsd.ns + "short", (short)5)]
        [TestCase("-20", xsd.ns + "short", (short)-20)]
        [TestCase("0", xsd.ns + "unsignedShort", (ushort)0)]
        [TestCase("5", xsd.ns + "unsignedShort", (ushort)5)]
        [TestCase("0", xsd.ns + "int", 0)]
        [TestCase("5", xsd.ns + "int", 5)]
        [TestCase("-20", xsd.ns + "int", -20)]
        [TestCase("0", xsd.ns + "unsignedInt", (uint)0)]
        [TestCase("5", xsd.ns + "unsignedInt", (uint)5)]
        [TestCase("0", xsd.ns + "long", (long)0)]
        [TestCase("5", xsd.ns + "long", (long)5)]
        [TestCase("-20", xsd.ns + "long", (long)-20)]
        [TestCase("0", xsd.ns + "unsignedLong", (ulong)0)]
        [TestCase("5", xsd.ns + "unsignedLong", (ulong)5)]
        [TestCase("0", xsd.ns + "integer", (long)0)]
        [TestCase("5", xsd.ns + "integer", (long)5)]
        [TestCase("-20", xsd.ns + "integer", (long)-20)]
        [TestCase("0", xsd.ns + "nonPositiveInteger", (long)0)]
        [TestCase("-20", xsd.ns + "nonPositiveInteger", (long)-20)]
        [TestCase("0", xsd.ns + "unsignedInteger", (long)0)]
        [TestCase("5", xsd.ns + "unsignedInteger", (long)5)]
        [TestCase("0", xsd.ns + "nonNegativeInteger", (ulong)0)]
        [TestCase("5", xsd.ns + "nonNegativeInteger", (ulong)5)]
        [TestCase("5", xsd.ns + "positiveInteger", (ulong)5)]
        public void Should_convert_from_literal(string value, string dataType, object expected)
        {
            Converter.ConvertFrom(StatementFor(value, new Iri(dataType))).Should().Be(expected);
        }

        [TestCase((sbyte)0, "0", xsd.ns + "byte")]
        [TestCase((sbyte)5, "5", xsd.ns + "byte")]
        [TestCase((sbyte)-20, "-20", xsd.ns + "byte")]
        [TestCase((byte)0, "0", xsd.ns + "unsignedByte")]
        [TestCase((byte)5, "5", xsd.ns + "unsignedByte")]
        [TestCase((short)0, "0", xsd.ns + "short")]
        [TestCase((short)5, "5", xsd.ns + "short")]
        [TestCase((short)-20, "-20", xsd.ns + "short")]
        [TestCase((ushort)0, "0", xsd.ns + "unsignedShort")]
        [TestCase((ushort)5, "5", xsd.ns + "unsignedShort")]
        [TestCase(0, "0", xsd.ns + "int")]
        [TestCase(5, "5", xsd.ns + "int")]
        [TestCase(-20, "-20", xsd.ns + "int")]
        [TestCase((uint)0, "0", xsd.ns + "unsignedInt")]
        [TestCase((uint)5, "5", xsd.ns + "unsignedInt")]
        [TestCase((long)0, "0", xsd.ns + "long")]
        [TestCase((long)5, "5", xsd.ns + "long")]
        [TestCase((long)-20, "-20", xsd.ns + "long")]
        [TestCase((ulong)0, "0", xsd.ns + "unsignedLong")]
        [TestCase((ulong)5, "5", xsd.ns + "unsignedLong")]
        public void Should_convert_to_literal(object value, string expectedLiteral, string dataType)
        {
            Converter.ConvertTo(Subject, Predicate, value).Should().MatchLiteralValueOf(expectedLiteral, new Iri(dataType));
        }

        [TestCase(xsd.ns + "byte")]
        [TestCase(xsd.ns + "unsignedByte")]
        [TestCase(xsd.ns + "short")]
        [TestCase(xsd.ns + "unsignedShort")]
        [TestCase(xsd.ns + "int")]
        [TestCase(xsd.ns + "unsignedInt")]
        [TestCase(xsd.ns + "long")]
        [TestCase(xsd.ns + "unsignedLong")]
        [TestCase(xsd.ns + "integer")]
        [TestCase(xsd.ns + "unsignedInteger")]
        [TestCase(xsd.ns + "nonPositiveInteger")]
        [TestCase(xsd.ns + "positiveInteger")]
        [TestCase(xsd.ns + "nonNegativeInteger")]
        public void Should_enlist_supported_data_types(string dataType)
        {
            Converter.SupportedDataTypes.Should().Contain(new Iri(dataType));
        }

        [TestCase(typeof(sbyte))]
        [TestCase(typeof(byte))]
        [TestCase(typeof(short))]
        [TestCase(typeof(ushort))]
        [TestCase(typeof(int))]
        [TestCase(typeof(uint))]
        [TestCase(typeof(long))]
        [TestCase(typeof(ulong))]
        public void Should_enlist_supported_type(Type type)
        {
            Converter.SupportedTypes.Should().Contain(type);
        }
    }
}