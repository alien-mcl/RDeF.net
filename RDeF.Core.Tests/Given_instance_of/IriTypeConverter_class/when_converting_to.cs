using System;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Entities;

namespace Given_instance_of.IriTypeConverter_class
{
    [TestFixture]
    public class when_converting_to : IriTypeConverterTest
    {
        private Uri Uri { get; set; }

        [Test]
        public void Should_confirm_it_can_convert_to_string()
        {
            Converter.CanConvertTo(typeof(string)).Should().BeTrue();
        }

        [Test]
        public void Should_confirm_it_can_convert_to_Uri()
        {
            Converter.CanConvertTo(typeof(Uri)).Should().BeTrue();
        }

        [Test]
        public void Should_convert_to_string()
        {
            Converter.ConvertTo(new Iri("some:Test"), typeof(string)).Should().Be("some:Test");
        }

        [Test]
        public void Should_provide_an_original_Uri_when_Iri_was_created_from_Uri()
        {
            Converter.ConvertTo(new Iri(Uri = new Uri("some:Test", UriKind.Absolute)), typeof(Uri)).Should().Be(Uri);
        }

        [Test]
        public void Should_convert_to_Uri()
        {
            Converter.ConvertTo(new Iri("some:Test"), typeof(Uri)).Should().BeOfType<Uri>().Which.ToString().Should().Be("some:Test");
        }

        [Test]
        public void Should_return_null_for_null_string_input()
        {
            Converter.ConvertTo(null, typeof(string)).Should().BeNull();
        }

        [Test]
        public void Should_return_null_for_non_Iri_input()
        {
            Converter.ConvertTo(0, typeof(Uri)).Should().BeNull();
        }

        [Test]
        public void Should_throw_for_other_type_conversions()
        {
            Converter.Invoking(instance => instance.ConvertTo(0, typeof(double)))
                .Should().Throw<NotSupportedException>();
        }
    }
}
