using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;

namespace Given_instance_of.DefaultEntityContext_class.which_is_using_an_in_memory_entity_source
{
    [TestFixture]
    public class when_querying : ScenarioTest
    {
        public override Task TheTest()
        {
            Context.AsQueryable<IProduct>();
            return Task.CompletedTask;
        }

        [Test]
        public void Should_throw_when_unsupported_entity_source_is_used()
        {
            new DefaultEntityContext(
                    new Mock<IEntitySource>(MockBehavior.Strict).Object,
                    MappingsRepository.Object,
                    ChangeDetector.Object,
                    new[] { LiteralConverter.Object })
                .Invoking(instance => instance.AsQueryable<IProduct>()).ShouldThrow<NotSupportedException>();
        }

        [Test]
        public void Should_obtain_a_queryable_entity_source_directly_from_it()
        {
            InMemoryEntitySource.Verify(instance => instance.AsQueryable<IProduct>(), Times.Once);
        }

        [Test]
        public void Should_obtain_an_underlying_entity_source()
        {
            Context.EntitySource.Should().Be(InMemoryEntitySource.Object);
        }

        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();
            InMemoryEntitySource.Setup(instance => instance.AsQueryable<IProduct>()).Returns(new IProduct[0].AsQueryable());
        }
    }
}
