using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RollerCaster;

namespace Given_instance_of.SimpleInMemoryEntitySource_class
{
    [TestFixture]
    public class when_creating_an_entity : SimpleInMemoryEntitySourceTest
    {
        private IProduct Result { get; set; }

        public override async Task TheTest()
        {
            Result = await EntitySource.Create<IProduct>(new Iri("test"));
        }

        [Test]
        public void Should_throw_when_no_iri_is_given()
        {
            EntitySource.Awaiting(instance => instance.Create<IProduct>(null))
                .Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("iri");
        }

        [Test]
        public void Should_create_new_entity_already_initialized()
        {
            ((Entity)Result.Unwrap()).IsInitialized.Should().BeTrue();
        }

        [Test]
        public void Should_create_new_entity_only_once()
        {
            EntitySource.Create<IProduct>(new Iri("test")).Result.Should().Be(Result);
        }
    }
}
