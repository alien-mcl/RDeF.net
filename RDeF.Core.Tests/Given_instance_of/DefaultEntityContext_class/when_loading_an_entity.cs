using System;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RollerCaster;

namespace Given_instance_of.DefaultEntityContext_class
{
    public class when_loading_an_entity : DefaultEntityContextTest
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
            Context.Invoking(context => context.Load<IProduct>(null)).ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Should_load_the_entity_only_once()
        {
            Context.Load<IProduct>(Iri).Unwrap().Should().Be(Product.Unwrap());
        }
    }
}
