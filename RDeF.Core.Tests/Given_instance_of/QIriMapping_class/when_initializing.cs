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
            ((QIriMapping)null).Invoking(_ => new QIriMapping(null, null))
                .Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("prefix");
        }

        [Test]
        public void Should_throw_when_an_empty_prefix_is_given()
        {
            ((QIriMapping)null).Invoking(_ => new QIriMapping(String.Empty, null))
                .Should().Throw<ArgumentOutOfRangeException>().Which.ParamName.Should().Be("prefix");
        }

        [Test]
        public void Should_throw_when_no_iri_is_given()
        {
            ((QIriMapping)null).Invoking(_ => new QIriMapping("test", null))
                .Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("iri");
        }
    }
}
