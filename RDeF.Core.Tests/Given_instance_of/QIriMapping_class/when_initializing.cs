using System;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Mapping;

namespace Given_instance_of.QIriMapping_class
{
    [TestFixture]
    public class when_initializing
    {
        [Test]
        public void Should_throw_when_no_prefix_is_given()
        {
            ((QIriMapping)null).Invoking(_ => new QIriMapping(null, null)).ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("prefix");
        }

        [Test]
        public void Should_throw_when_an_empty_prefix_is_given()
        {
            ((QIriMapping)null).Invoking(_ => new QIriMapping(String.Empty, null)).ShouldThrow<ArgumentOutOfRangeException>().Which.ParamName.Should().Be("prefix");
        }

        [Test]
        public void Should_throw_when_no_iri_is_given()
        {
            ((QIriMapping)null).Invoking(_ => new QIriMapping("test", null)).ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("iri");
        }
    }
}
