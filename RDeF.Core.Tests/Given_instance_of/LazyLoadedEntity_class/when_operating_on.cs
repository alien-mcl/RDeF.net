using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Entities;
using RollerCaster;

namespace Given_instance_of.LazyLoadedEntity_class
{
    [TestFixture]
    public class when_operating_on
    {
        private static readonly Iri SomePredicate = new Iri("some:predicate");
        private static readonly Iri SomeResource = new Iri("some:resource");

        private Mock<IEntityContext> Context { get; set; }

        private LazyLoadedEntity Entity { get; set; }

        private IEntity RealEntity { get; set; }

        private Relation Relation { get; set; }

        private IEnumerable<Relation> Result { get; set; }

        public void TheTest()
        {
            Result = Entity.UnmappedRelations;
        }

        [Test]
        public void Should_provide_correct_iri()
        {
            Entity.Iri.Should().Be(SomeResource);
        }

        [Test]
        public void Should_provide_correct_unmapped_properties()
        {
            Result.ShouldBeEquivalentTo(new[] { Relation });
        }

        [Test]
        public void Should_provide_correct_context()
        {
            Entity.Context.Should().Be(Context.Object);
        }

        [Test]
        public void Should_provide_correct_proxy()
        {
            ((IProxy)Entity).WrappedObject.Should().Be(RealEntity.Unwrap());
        }

        [Test]
        public void Should_provide_correct_casted_type()
        {
            ((IProxy)Entity).CurrentCastedType.Should().Be(typeof(IEntity));
        }

        [SetUp]
        public void Setup()
        {
            Context = new Mock<IEntityContext>(MockBehavior.Strict);
            Entity = new LazyLoadedEntity(Context.Object, SomeResource);
            Relation = new Relation(SomePredicate, Entity);
            Entity.Relation = Relation;
            var realEntity = new MulticastObject();
            realEntity.SetProperty(typeof(IEntity).GetProperty(nameof(IEntity.UnmappedRelations)), new[] { Relation });
            realEntity.SetProperty(typeof(IEntity).GetProperty(nameof(IEntity.Context)), Context.Object);
            RealEntity = realEntity.ActLike<IEntity>();
            Context.Setup(_ => _.Load<IEntity>(It.IsAny<Iri>())).Returns(RealEntity);
            TheTest();
        }
    }
}
