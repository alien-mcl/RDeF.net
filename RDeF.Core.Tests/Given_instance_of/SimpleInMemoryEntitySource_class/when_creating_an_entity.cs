using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Mapping;
using RollerCaster;

namespace Given_instance_of.SimpleInMemoryEntitySource_class
{
    [TestFixture]
    public class when_creating_an_entity : SimpleInMemoryEntitySourceTest
    {
        private DefaultEntityContext Context { get; set; }

        private IProduct Result { get; set; }

        public override void TheTest()
        {
            Result = EntitySource.Create<IProduct>(new Iri("test"), Context);
        }

        [Test]
        public void Should_throw_when_no_iri_is_given()
        {
            EntitySource.Invoking(instance => instance.Create<IProduct>(null, null)).ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("iri");
        }

        [Test]
        public void Should_throw_when_no_entity_context_is_given()
        {
            EntitySource.Invoking(instance => instance.Create<IProduct>(new Iri("test"), null)).ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("entityContext");
        }

        [Test]
        public void Should_throw_when_invalid_entity_context_is_given()
        {
            EntitySource.Invoking(instance => instance.Create<IProduct>(new Iri("test"), new Mock<IEntityContext>(MockBehavior.Strict).Object))
                .ShouldThrow<ArgumentOutOfRangeException>().Which.ParamName.Should().Be("entityContext");
        }

        [Test]
        public void Should_create_new_entity_already_initialized()
        {
            ((Entity)Result.Unwrap()).IsInitialized.Should().BeTrue();
        }

        [Test]
        public void Should_create_new_entity_only_once()
        {
            EntitySource.Create<IProduct>(new Iri("test"), Context).Should().Be(Result);
        }

        protected override void ScenarioSetup()
        {
            Context = new DefaultEntityContext(
                EntitySource,
                new Mock<IMappingsRepository>(MockBehavior.Strict).Object,
                new Mock<IChangeDetector>(MockBehavior.Strict).Object,
                type => null);
        }
    }
}
