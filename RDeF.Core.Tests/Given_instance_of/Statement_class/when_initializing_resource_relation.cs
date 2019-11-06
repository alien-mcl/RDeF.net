using System;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Entities;

namespace Given_instance_of.Statement_class
{
    [TestFixture]
    public class when_initializing_resource_relation
    {
        [Test]
        public void Should_throw_when_no_subject_is_given()
        {
            ((Statement)null).Invoking(_ => new Statement((Iri)null, (Iri)null, (Iri)null))
                .Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("subject");
        }

        [Test]
        public void Should_throw_when_no_predicate_is_given()
        {
            ((Statement)null).Invoking(_ => new Statement(new Iri("test"), (Iri)null, (Iri)null))
                .Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("predicate");
        }

        [Test]
        public void Should_throw_when_no_object_is_given()
        {
            ((Statement)null).Invoking(_ => new Statement(new Iri("test"), new Iri("test"), (Iri)null))
                .Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("object");
        }
    }
}
