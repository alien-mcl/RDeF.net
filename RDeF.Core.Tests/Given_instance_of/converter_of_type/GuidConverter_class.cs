using System;
using System.Text.RegularExpressions;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Entities;
using RDeF.Mapping.Converters;
using RDeF.Vocabularies;

namespace Given_instance_of.converter_of_type
{
    [TestFixture]
    public class GuidConverter_class : LiteralConverterTest<GuidConverter>
    {
        [TestCase("6cbb1450-c74d-47e8-a1e0-50f1db6cfd6f")]
        [TestCase("{6cbb1450-c74d-47e8-a1e0-50f1db6cfd6f}")]
        [TestCase("(6cbb1450-c74d-47e8-a1e0-50f1db6cfd6f)")]
        public void Should_convert_from_literal(string value)
        {
            Converter.ConvertFrom(StatementFor(value, oguid.Namespace)).Should().Be(Guid.Parse(Regex.Replace(value, "[{()}]", String.Empty)));
        }

        [Test]
        public void Should_throw_when_converting_to_literal()
        {
            Converter.Invoking(instance => instance.ConvertTo(Subject, Predicate, Guid.Empty))
                .Should().Throw<NotSupportedException>();
        }

        [TestCase(oguid.ns)]
        public void Should_enlist_supported_data_types(string dataType)
        {
            Converter.SupportedDataTypes.Should().Contain(new Iri(dataType));
        }

        [Test]
        public void Should_not_enlist_any_supported_type()
        {
            Converter.SupportedTypes.Should().BeEmpty();
        }
    }
}