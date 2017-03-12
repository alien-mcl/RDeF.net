using System;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RollerCaster;

namespace Given_instance_of.DefaultEntityContext_class
{
    public class when_creating_an_entity : DefaultEntityContextTest
    {
        private static readonly Iri Iri = new Iri(new Uri("http://test.com/"));

        private IProduct Product { get; set; }

        public override void TheTest()
        {
            Product = Context.Create<IProduct>(Iri);
        }

        [Test]
        public void Should_throw_when_no_Iri_is_given()
        {
            Context.Invoking(context => context.Create<IProduct>(null)).ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Should_create_the_entity_only_once()
        {
            Context.Create<IProduct>(Iri).Unwrap().Should().Be(Product.Unwrap());
        }

        [Test]
        public void Should_make_the_entity_initialized()
        {
            ((Entity)Product.Unwrap()).IsInitialized.Should().BeTrue();
        }
    }
}
