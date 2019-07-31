using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Entities;

namespace Given_instance_of.SimpleInMemoryEntitySource_class
{
    [TestFixture]
    public class when_deleting : SimpleInMemoryEntitySourceTest
    {
        private static readonly Iri Iri = new Iri("test");

        private Entity Entity { get; set; }

        public override void TheTest()
        {
            EntitySource.Delete(Iri);
        }

        [Test]
        public void Should_throw_when_no_iri_is_given()
        {
            EntitySource.Invoking(instance => instance.Delete(null)).ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Should_remove_the_entity_from_cache()
        {
            EntitySource.Entities.Should().NotContainKey(Entity);
        }

        [Test]
        public void Should_completely_remove_the_entity_from_cache()
        {
            EntitySource.EntityMap.Should().NotContainKey(Iri);
        }

        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();
            EntitySource.Entities[EntitySource.EntityMap[Iri] = Entity = new Entity(Iri, Context.Object)] = new HashSet<Statement>();
        }
    }
}
