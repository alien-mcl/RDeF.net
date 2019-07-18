using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Entities;

namespace Given_instance_of.Relation_struct
{
    [TestFixture]
    public class when_initializing
    {
        [Test]
        public void Should_throw_when_no_predicate_is_given()
        {
            ((Relation)null).Invoking(_ => new Relation(null, (IEntity)null))
                .ShouldThrow<ArgumentNullException>()
                .Which.ParamName.Should().Be("predicate");
        }

        [Test]
        public void Should_throw_when_no_entity_is_given()
        {
            ((Relation)null).Invoking(_ => new Relation(new Iri(), (IEntity)null))
                .ShouldThrow<ArgumentNullException>()
                .Which.ParamName.Should().Be("object");
        }

        [Test]
        public void Should_throw_when_provided_entity_has_no_Iri()
        {
            ((Relation)null).Invoking(_ => new Relation(new Iri(), new Mock<IEntity>().Object))
                .ShouldThrow<ArgumentOutOfRangeException>()
                .Which.ParamName.Should().Be("object");
        }

        [Test]
        public void Should_throw_when_no_property_is_given()
        {
            ((Relation)null).Invoking(_ => new Relation(null, (object)null))
                .ShouldThrow<ArgumentNullException>()
                .Which.ParamName.Should().Be("predicate");
        }

        [Test]
        public void Should_throw_when_no_value_is_given()
        {
            ((Relation)null).Invoking(_ => new Relation(new Iri(), (object)null))
                .ShouldThrow<ArgumentNullException>()
                .Which.ParamName.Should().Be("value");
        }
    }
}
