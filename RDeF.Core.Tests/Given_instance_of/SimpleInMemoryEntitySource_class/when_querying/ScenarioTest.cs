using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Mapping;
using RollerCaster;

namespace Given_instance_of.SimpleInMemoryEntitySource_class.when_querying
{
    public abstract class ScenarioTest<T> : SimpleInMemoryEntitySourceTest where T : IEntity
    {
        protected Mock<IMappingsRepository> MappingsRepository { get; private set; }

        private IQueryable<T> Result { get; set; }

        public override Task TheTest()
        {
            Result = EntitySource.AsQueryable<T>();
            return Task.CompletedTask;
        }

        [Test]
        public void Should_provide_a_queryable_collection_of_strongly_typed_entities()
        {
            Result.Should().HaveCount(1);
        }

        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();
            var entity = new Entity(new Iri("test"), Context.Object);
            EntitySource.Entities[EntitySource.EntityMap[entity.Iri] = entity] = new HashSet<Statement>();
            MappingsRepository = new Mock<IMappingsRepository>(MockBehavior.Strict);
            MappingsRepository.As<IEnumerable<IEntityMapping>>();
            Context.SetupGet(_ => _.Mappings).Returns(MappingsRepository.Object);
            Context.Setup(_ => _.InitializeInternal(
                It.IsAny<Entity>(),
                It.IsAny<IEnumerable<Statement>>(),
                It.IsAny<EntityInitializationContext>(),
                It.IsAny<Action<Statement>>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            Context.Setup(_ => _.Initialize(It.IsAny<Entity>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            entity.ActLike<IProduct>();
        }
    }
}
