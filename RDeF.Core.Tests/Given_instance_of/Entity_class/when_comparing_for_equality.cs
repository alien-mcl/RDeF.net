using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RollerCaster;

namespace Given_instance_of.Entity_class
{
    [TestFixture]
    public class when_comparing_for_equality : EntityTest
    {
        private Mock<IEntity> AnotherEntity { get; set; }

        [Test]
        public void Should_consider_as_equal_same_Entity_instance()
        {
            Entity.Equals(Entity).Should().BeTrue();
        }

        [Test]
        public void Should_consider_as_equal_entities_with_same_Iri()
        {
            Entity.Equals(new Entity(Iri, Context.Object)).Should().BeTrue();
        }

        [Test]
        public void Should_consider_as_equal_same_instance_of_a_casted_entity()
        {
            Entity.Equals(Entity.ActLike<IProduct>()).Should().BeTrue();
        }

        [Test]
        public void Should_consider_as_equal_different_entities_with_same_Iri()
        {
            Entity.Equals(AnotherEntity.Object).Should().BeTrue();
        }

        protected override void ScenarioSetup()
        {
            AnotherEntity = new Mock<IEntity>(MockBehavior.Strict);
            AnotherEntity.SetupGet(instance => instance.Iri).Returns(Iri);
        }
    }
}
