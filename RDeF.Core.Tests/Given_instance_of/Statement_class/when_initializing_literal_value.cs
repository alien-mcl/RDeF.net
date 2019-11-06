using System;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Entities;

namespace Given_instance_of.Statement_class
{
    [TestFixture]
    public class when_initializing_literal_value
    {
        [Test]
        public void Should_throw_when_no_subject_is_given()
        {
            ((Statement)null).Invoking(_ => new Statement((Iri)null, (Iri)null, (string)null))
                .Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("subject");
        }

        [Test]
        public void Should_throw_when_no_predicate_is_given()
        {
            ((Statement)null).Invoking(_ => new Statement(new Iri("test"), (Iri)null, (string)null))
                .Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("predicate");
        }

        [Test]
        public void Should_throw_when_no_value_is_given()
        {
            ((Statement)null).Invoking(_ => new Statement(new Iri("test"), new Iri("test"), (string)null))
                .Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("value");
        }
    }
}
