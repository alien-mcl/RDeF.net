using System;
using System.Threading.Tasks;
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

        public override Task TheTest()
        {
            Product = Context.Create<IProduct>(Iri);
            return Task.CompletedTask;
        }

        [Test]
        public void Should_throw_when_no_Iri_is_given()
        {
            Context.Awaiting(context => context.Load<IProduct>(null))
                .Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Should_load_the_entity_only_once()
        {
            Context.Load<IProduct>(Iri).Result.Unwrap().Should().Be(Product.Unwrap());
        }
    }
}
