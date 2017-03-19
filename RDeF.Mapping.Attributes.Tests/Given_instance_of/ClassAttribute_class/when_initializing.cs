using System;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Mapping.Attributes;

namespace Given_instance_of.ClassAttribute_class
{
    [TestFixture]
    public class when_initializing
    {
        [Test]
        public void Should_throw_when_no_prefix_is_given()
        {
            ((ClassAttribute)null).Invoking(_ => new ClassAttribute(null, null))
                .ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("prefix");
        }

        [Test]
        public void Should_throw_when_an_empty_prefix_is_given()
        {
            ((ClassAttribute)null).Invoking(_ => new ClassAttribute(String.Empty, null))
                .ShouldThrow<ArgumentOutOfRangeException>().Which.ParamName.Should().Be("prefix");
        }

        [Test]
        public void Should_throw_when_no_term_is_given()
        {
            ((ClassAttribute)null).Invoking(_ => new ClassAttribute("prefix", null))
                .ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("term");
        }

        [Test]
        public void Should_throw_when_an_empty_term_is_given()
        {
            ((ClassAttribute)null).Invoking(_ => new ClassAttribute("prefix", String.Empty))
                .ShouldThrow<ArgumentOutOfRangeException>().Which.ParamName.Should().Be("term");
        }

        [Test]
        public void Should_throw_when_no_Iri_given()
        {
            ((ClassAttribute)null).Invoking(_ => new ClassAttribute() { Iri = null })
                .ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("iri");
        }

        [Test]
        public void Should_throw_when_an_empty_Iri_is_given()
        {
            ((ClassAttribute)null).Invoking(_ => new ClassAttribute() { Iri = String.Empty })
                .ShouldThrow<ArgumentOutOfRangeException>().Which.ParamName.Should().Be("iri");
        }
    }
}
