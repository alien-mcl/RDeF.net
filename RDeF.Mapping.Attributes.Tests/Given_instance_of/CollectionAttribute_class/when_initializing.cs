using System;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Mapping.Attributes;

namespace Given_instance_of.CollectionAttribute_class
{
    [TestFixture]
    public class when_initializing
    {
        [Test]
        public void Should_throw_when_no_prefix_is_given()
        {
            ((CollectionAttribute)null).Invoking(_ => new CollectionAttribute(null, null))
                .ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("prefix");
        }

        [Test]
        public void Should_throw_when_an_empty_prefix_is_given()
        {
            ((CollectionAttribute)null).Invoking(_ => new CollectionAttribute(String.Empty, null))
                .ShouldThrow<ArgumentOutOfRangeException>().Which.ParamName.Should().Be("prefix");
        }

        [Test]
        public void Should_throw_when_no_term_is_given()
        {
            ((CollectionAttribute)null).Invoking(_ => new CollectionAttribute("prefix", null))
                .ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("term");
        }

        [Test]
        public void Should_throw_when_an_empty_term_is_given()
        {
            ((CollectionAttribute)null).Invoking(_ => new CollectionAttribute("prefix", String.Empty))
                .ShouldThrow<ArgumentOutOfRangeException>().Which.ParamName.Should().Be("term");
        }

        [Test]
        public void Should_throw_when_wrong_converter_type_is_given()
        {
            ((CollectionAttribute)null).Invoking(_ => new CollectionAttribute() { ValueConverterType = typeof(string) })
                .ShouldThrow<ArgumentOutOfRangeException>().Which.ParamName.Should().Be("valueConverterType");
        }

        [Test]
        public void Should_throw_when_no_Iri_given()
        {
            ((CollectionAttribute)null).Invoking(_ => new CollectionAttribute() { Iri = null })
                .ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("iri");
        }

        [Test]
        public void Should_throw_when_an_empty_Iri_is_given()
        {
            ((CollectionAttribute)null).Invoking(_ => new CollectionAttribute() { Iri = String.Empty })
                .ShouldThrow<ArgumentOutOfRangeException>().Which.ParamName.Should().Be("iri");
        }
    }
}
