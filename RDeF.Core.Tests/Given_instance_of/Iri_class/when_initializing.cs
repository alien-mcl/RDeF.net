using System;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Entities;

namespace Given_instance_of.Iri_class
{
    [TestFixture]
    public class when_initializing
    {
        [Test]
        public void Should_throw_when_no_iri_string_is_given()
        {
            ((Iri)null).Invoking(_ => new Iri((string)null)).ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Should_throw_when_an_empty_iri_string_is_given()
        {
            ((Iri)null).Invoking(_ => new Iri(String.Empty)).ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Should_throw_when_no_Uri_is_given()
        {
            ((Iri)null).Invoking(_ => new Iri((Uri)null)).ShouldThrow<ArgumentNullException>();
        }
    }
}
