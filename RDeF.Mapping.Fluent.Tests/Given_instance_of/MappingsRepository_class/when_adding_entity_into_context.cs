using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Entities;
using RDeF.Mapping;
using RDeF.Mapping.Entities;
using RDeF.Mapping.Explicit;

namespace Given_instance_of.MappingsRepository_class
{
    [TestFixture]
    public class when_adding_entity_into_context
    {
        private Mock<IMappingsRepository> MappingsRepository { get; set; }

        private Mock<IEntity> Entity { get; set; }

        private Mock<IEntity> UnmappedEntity { get; set; }

        private Mock<IEntityContext> Context { get; set; }

        private Mock<IExplicitMappings> ExplicitMappings { get; set; }

        [Test]
        public void Should_return_null_if_no_mappings_repository_is_given()
        {
            ((IMappingsRepository)null).IncludingMappingsFor(null).Should().BeNull();
        }

        [Test]
        public void Should_return_given_mappings_repository_if_no_owning_entity_is_given()
        {
            ((object)MappingsRepository.Object.IncludingMappingsFor(null)).Should().Be(MappingsRepository.Object);
        }

        [Test]
        public void Should_return_given_mappings_repository_if_no_entity_mappings_are_defined()
        {
            ((object)MappingsRepository.Object.IncludingMappingsFor(UnmappedEntity.Object)).Should().Be(MappingsRepository.Object);
        }

        [Test]
        public void Should_return_entity_aware_mappings_repository_if_there_are_any_entity_mappings_defined()
        {
            MappingsRepository.Object.IncludingMappingsFor(Entity.Object).Should().BeOfType<EntityAwareMappingsRepository>();
        }

        [SetUp]
        public void Setup()
        {
            MappingsRepository = new Mock<IMappingsRepository>(MockBehavior.Strict);
            ExplicitMappings = new Mock<IExplicitMappings>(MockBehavior.Strict);
            Context = new Mock<IEntityContext>(MockBehavior.Strict);
            Entity = new Mock<IEntity>(MockBehavior.Strict);
            Entity.SetupGet(instance => instance.Context).Returns(Context.Object);
            Entity.SetupGet(instance => instance.Iri).Returns(new Iri());
            EntityContextExtensions.ExplicitMappings[Context.Object] = ExplicitMappings.Object;
            UnmappedEntity = new Mock<IEntity>(MockBehavior.Strict);
            UnmappedEntity.SetupGet(instance => instance.Context).Returns(new Mock<IEntityContext>().Object);
        }

        [TearDown]
        public void Teardown()
        {
            EntityContextExtensions.ExplicitMappings.Clear();
        }
    }
}
