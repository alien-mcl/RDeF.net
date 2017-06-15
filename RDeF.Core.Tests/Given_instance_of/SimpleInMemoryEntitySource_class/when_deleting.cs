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
        private Entity Entity { get; set; }

        public override void TheTest()
        {
            EntitySource.Delete(new Iri("test"));
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

        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();
            EntitySource.Entities[Entity = new Entity(new Iri("test"), Context.Object)] = new HashSet<Statement>();
        }
    }
}
