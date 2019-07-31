using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RollerCaster;

namespace Given_instance_of.SimpleInMemoryEntitySource_class.when_querying
{
    public abstract class ScenarioTest<T> : SimpleInMemoryEntitySourceTest where T : IEntity
    {
        private IQueryable<T> Result { get; set; }

        public override void TheTest()
        {
            Result = EntitySource.AsQueryable<T>();
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
            entity.ActLike<IProduct>();
        }
    }
}
