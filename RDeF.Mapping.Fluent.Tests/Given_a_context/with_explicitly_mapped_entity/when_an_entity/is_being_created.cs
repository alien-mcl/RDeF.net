using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Mapping.Entities;

namespace Given_a_context.with_explicitly_mapped_entity.when_an_entity
{
    [TestFixture]
    public class is_being_created : ScenarioTest
    {
        public override Task TheTest()
        {
            Context.Create<IUnmappedProduct>(new Iri("test"), MapPrimaryEntity);
            return Task.CompletedTask;
        }

        [Test]
        public void Should_throw_when_no_entity_context_is_given()
        {
            ((IEntityContext)null).Invoking(_ => _.Create<IUnmappedProduct>(null, null))
                .Should().Throw<ArgumentNullException>();
        }

        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();
            EntityContext.Setup(instance => instance.Create<IUnmappedProduct>(It.IsAny<Iri>()))
                .Returns(new Mock<IUnmappedProduct>(MockBehavior.Strict).Object);
        }
    }
}
