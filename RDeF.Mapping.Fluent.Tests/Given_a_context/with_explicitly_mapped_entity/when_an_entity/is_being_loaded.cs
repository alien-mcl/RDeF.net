using System;
using System.Threading;
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
    public class is_being_loaded : ScenarioTest
    {
        public override Task TheTest()
        {
            return Context.Load<IUnmappedProduct>(new Iri("test"), MapPrimaryEntity);
        }

        [Test]
        public void Should_throw_when_no_entity_context_is_given()
        {
            ((IEntityContext)null).Awaiting(_ => _.Load<IUnmappedProduct>(null, null))
                .Should().Throw<ArgumentNullException>();
        }

        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();
            EntityContext.Setup(instance => instance.Load<IUnmappedProduct>(It.IsAny<Iri>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Mock<IUnmappedProduct>(MockBehavior.Strict).Object);
        }
    }
}
