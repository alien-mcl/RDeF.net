using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Entities;
using RDeF.Mapping;

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
            var entityContext = new DefaultEntityContext(
                EntitySource,
                new Mock<IMappingsRepository>(MockBehavior.Strict).Object,
                new Mock<IChangeDetector>(MockBehavior.Strict).Object,
                type => null);
            EntitySource.Entities[Entity = new Entity(new Iri("test"), entityContext)] = new HashSet<Statement>();
        }
    }
}
