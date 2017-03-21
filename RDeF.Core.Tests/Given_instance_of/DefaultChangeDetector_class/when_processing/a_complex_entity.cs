using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF;
using RDeF.Data;
using RDeF.Entities;
using RollerCaster;

namespace Given_instance_of.DefaultChangeDetector_class.when_processing
{
    [TestFixture]
    internal class a_complex_entity : ScenarioTest
    {
        private IComplexEntity ComplexEntity { get; set; }

        public override void TheTest()
        {
            Detector.Process(Entity, RetractedStatements, AddedStatements);
        }

        [Test]
        public void Should_retract_previous_ordinals_values()
        {
            RetractedStatements.Should().ContainKey(Entity).WhichValue.Should().ContainValuesForPropertyOf<IComplexEntity, int>("Ordinals", 1, 2);
        }

        [Test]
        public void Should_add_new_ordinals_values()
        {
            AddedStatements.Should().ContainKey(Entity).WhichValue.Should().ContainValuesForPropertyOf<IComplexEntity, int>("Ordinals", 2, 3);
        }

        [Test]
        public void Should_retract_previous_doubles_list()
        {
            RetractedStatements.Should().ContainKey(Entity).WhichValue.Should().ContainLinkedListForPropertyOf<IComplexEntity, double>("Doubles", 2.71828, 3.14159);
        }

        [Test]
        public void Should_add_new_doubles_list()
        {
            AddedStatements.Should().ContainKey(Entity).WhichValue.Should().ContainLinkedListForPropertyOf<IComplexEntity, double>("Doubles", 0.1, 0.2);
        }

        [Test]
        public void Should_retract_previous_related_values()
        {
            RetractedStatements.Should().ContainKey(Entity).WhichValue.Should()
                .ContainValuesForPropertyOf<IComplexEntity, IComplexEntity>("Related", ComplexEntity, ComplexEntity);
        }

        [Test]
        public void Should_add_new_related_values()
        {
            AddedStatements.Should().ContainKey(Entity).WhichValue.Should()
                .ContainValuesForPropertyOf<IComplexEntity, IComplexEntity>("Related", MockForEntityOfType<IComplexEntity>("other"));
        }

        [Test]
        public void Should_retract_previous_other_list()
        {
            RetractedStatements.Should().ContainKey(Entity).WhichValue.Should()
                .ContainLinkedListForPropertyOf<IComplexEntity, IComplexEntity>("Other", ComplexEntity, ComplexEntity);
        }

        [Test]
        public void Should_add_new_other_list()
        {
            AddedStatements.Should().ContainKey(Entity).WhichValue.Should()
                .ContainLinkedListForPropertyOf<IComplexEntity, IComplexEntity>("Other", MockForEntityOfType<IComplexEntity>("other"));
        }

        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();
            Initialize(Entity);
        }

        private void Initialize(Entity entity)
        {
            ComplexEntity = entity.ActLike<IComplexEntity>();
            ComplexEntity.Ordinals.Add(1);
            ComplexEntity.Ordinals.Add(2);
            ComplexEntity.Doubles.Add(2.71828);
            ComplexEntity.Doubles.Add(3.14159);
            ComplexEntity.Related.Add(ComplexEntity);
            ComplexEntity.Related.Add(ComplexEntity);
            ComplexEntity.Other.Add(ComplexEntity);
            ComplexEntity.Other.Add(ComplexEntity);
            Entity.IsInitialized = true;
            ComplexEntity.Ordinals.Clear();
            ComplexEntity.Ordinals.Add(2);
            ComplexEntity.Ordinals.Add(3);
            ComplexEntity.Doubles.Clear();
            ComplexEntity.Doubles.Add(0.1);
            ComplexEntity.Doubles.Add(0.2);
            var otherEntity = new Entity(new Iri("other"), Context.Object);
            var otherProduct = otherEntity.ActLike<IComplexEntity>();
            ComplexEntity.Related.Clear();
            ComplexEntity.Related.Add(otherProduct);
            ComplexEntity.Other.Clear();
            ComplexEntity.Other.Add(otherProduct);
        }

        private TEntity MockForEntityOfType<TEntity>(string iri) where TEntity : class, IEntity
        {
            var result = new Mock<TEntity>(MockBehavior.Strict);
            result.SetupGet(instance => instance.Iri).Returns(new Iri(iri));
            return result.Object;
        }
    }
}
