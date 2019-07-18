using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Entities;

namespace Given_instance_of.Relation_struct
{
    [TestFixture]
    public class when_comparing_for_equality
    {
        private const int Literal = 0;
        private static readonly Iri SomeGraph = new Iri("some:graph");
        private static readonly Iri SomePredicate = new Iri("some:predicate");
        private static readonly Iri SomeResource = new Iri("some:resource");

        private static Relation Null
        {
            get { return null; }
        }

        private Mock<IEntity> Entity { get; set; }

        private Relation Relation { get; set; }

        private Relation Value { get; set; }

        [Test]
        public void Should_acknowledge_same_null_relations_as_equal()
        {
            (Null == Null).Should().BeTrue();
        }

        [Test]
        public void Should_acknowledge_relation_and_null_as_unequal()
        {
            (Relation != Null).Should().BeTrue();
        }
        
        [Test]
        public void Should_acknowledge_null_and_relation_as_unequal()
        {
            (Null != Relation).Should().BeTrue();
        }

        [Test]
        public void Should_acknowledge_different_relations_as_unequal()
        {
            (Relation != Value).Should().BeTrue();
        }

        [Test]
        public void Should_acknowledge_same_relations_equal()
        {
            Relation.Equals((object)new Relation(SomePredicate, Entity.Object, SomeGraph)).Should().BeTrue();
        }

        [Test]
        public void Should_acknowledge_same_properties_equal()
        {
            Value.Equals((object)new Relation(SomePredicate, Literal, SomeGraph)).Should().BeTrue();
        }

        [Test]
        public void Should_acknowledge_same_relations_as_equal()
        {
            (Relation == new Relation(SomePredicate, Entity.Object, SomeGraph)).Should().BeTrue();
        }

        [Test]
        public void Should_acknowledge_same_properties_as_equal()
        {
            (Value == new Relation(SomePredicate, Literal, SomeGraph)).Should().BeTrue();
        }

        [Test]
        public void Should_initialize_object()
        {
            Relation.Initialize(null).Object.Should().BeNull();
        }

        [SetUp]
        public void Setup()
        {
            Entity = new Mock<IEntity>(MockBehavior.Strict);
            Entity.SetupGet(_ => _.Iri).Returns(SomeResource);
            Relation = new Relation(SomePredicate, Entity.Object, SomeGraph);
            Value = new Relation(SomePredicate, Literal, SomeGraph);
        }
    }
}
