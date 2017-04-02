using System;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Mapping;

namespace Given_instance_of.AmbiguousMappingException_class
{
    [TestFixture]
    public class when_initializing
    {
        [Test]
        public void Should_not_throw_when_using_default_constructor()
        {
            ((AmbiguousMappingException)null).Invoking(_ => new AmbiguousMappingException()).ShouldNotThrow();
        }

        [Test]
        public void Should_not_throw_when_providing_a_message()
        {
            ((AmbiguousMappingException)null).Invoking(_ => new AmbiguousMappingException("Some message")).ShouldNotThrow();
        }

        [Test]
        public void Should_not_throw_when_providing_a_message_with_inner_exception()
        {
            ((AmbiguousMappingException)null).Invoking(_ => new AmbiguousMappingException("Some message", new Exception())).ShouldNotThrow();
        }
    }
}
