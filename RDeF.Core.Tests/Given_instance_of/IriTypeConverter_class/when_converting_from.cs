using System;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Entities;

namespace Given_instance_of.IriTypeConverter_class
{
    [TestFixture]
    public class when_converting_from : IriTypeConverterTest
    {
        [Test]
        public void Should_confirm_it_can_convert_from_string()
        {
            Converter.CanConvertFrom(typeof(string)).Should().BeTrue();
        }

        [Test]
        public void Should_confirm_it_can_convert_from_Uri()
        {
            Converter.CanConvertFrom(typeof(Uri)).Should().BeTrue();
        }

        [Test]
        public void Should_convert_from_string()
        {
            Converter.ConvertFrom("Test").Should().Be(new Iri("Test"));
        }

        [Test]
        public void Should_convert_from_Uri()
        {
            Converter.ConvertFrom(new Uri("Test", UriKind.Relative)).ToString().Should().Be("Test");
        }

        [Test]
        public void Should_return_null_for_null_input()
        {
            Converter.ConvertFrom(null).Should().BeNull();
        }

        [Test]
        public void Should_throw_for_other_type_conversions()
        {
            Converter.Invoking(instance => instance.ConvertTo(0, typeof(double)))
                .Should().Throw<NotSupportedException>();
        }
    }
}
