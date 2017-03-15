using System.Collections.Generic;
using System.Linq;
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
    public class when_querying : SimpleInMemoryEntitySourceTest
    {
        private IQueryable<IProduct> Result { get; set; }

        public override void TheTest()
        {
            Result = EntitySource.AsQueryable<IProduct>();
        }

        [Test]
        public void Should_provide_a_queryable_collection_of_typed_entities()
        {
            Result.Should().HaveCount(1);
        }

        [Test]
        public void Should_provide_a_queryable_collection_of_entities()
        {
            EntitySource.AsQueryable<IEntity>().Should().HaveCount(1);
        }

        protected override void ScenarioSetup()
        {
            var entityContext = new DefaultEntityContext(EntitySource, new Mock<IMappingsRepository>(MockBehavior.Strict).Object, new Mock<IChangeDetector>(MockBehavior.Strict).Object);
            var entity = new Entity(new Iri("test"), entityContext);
            EntitySource.Entities[entity] = new HashSet<Statement>();
            entity.ActLike<IProduct>();
        }
    }
}
