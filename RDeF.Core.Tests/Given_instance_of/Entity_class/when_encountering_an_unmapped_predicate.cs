using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Mapping;
using RDeF.Vocabularies;
using RollerCaster;

namespace Given_instance_of.Entity_class
{
    [TestFixture]
    public class when_encountering_an_unmapped_predicate : EntityPropertyTest
    {
        private static readonly Iri AnotherIri = new Iri("another:iri");
        private static readonly Iri SomePredicate = new Iri("some:predicate");
        private static readonly Iri SomeAnotherPredicate = new Iri("some:another-predicate");
        private static readonly Iri YetAnotherPredicate = new Iri("yet:another-predicate");

        private IProduct Product { get; set; }

        private IProduct RelatedProduct { get; set; }

        private IEnumerable<Relation> Result { get; set; }

        public override void TheTest()
        {
            Result = Product.UnmappedRelations;
        }

        [Test]
        public void Should_provide_correct_unmapped_property()
        {
            Result.ShouldBeEquivalentTo(new[]
            {
                new Relation(SomePredicate, "Test"),
                new Relation(SomeAnotherPredicate, RelatedProduct),
                new Relation(YetAnotherPredicate, "Raw data")
            });
        }

        [Test]
        public void Should_use_literal_converter()
        {
            LiteralConverter.Verify(
                instance => instance.ConvertFrom(It.Is<Statement>(statement => statement.Predicate == SomePredicate)),
                Times.Once);
        }

        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();
            MappingsRepository.Setup(instance => instance.FindPropertyMappingFor(It.IsAny<IEntity>(), It.IsAny<Iri>(), It.IsAny<Iri>()))
                .Returns((IPropertyMapping)null);
            EntitySource.Setup(instance => instance.Load(Iri))
                .Returns<Iri>(iri => new[]
                {
                    new Statement(iri, SomePredicate, "Test", xsd.@string),
                    new Statement(iri, SomeAnotherPredicate, AnotherIri),
                    new Statement(iri, YetAnotherPredicate, "Raw data")
                });
            EntitySource.Setup(instance => instance.Load(AnotherIri)).Returns<Iri>(iri => new Statement[0]);
            LiteralConverter.SetupGet(instance => instance.SupportedDataTypes).Returns(new[] { xsd.@string });
            LiteralConverter.Setup(instance => instance.ConvertFrom(It.IsAny<Statement>()))
                .Returns<Statement>(statement => statement.Value);
            Context.CallBase = true;
            Product = Entity.ActLike<IProduct>();
            RelatedProduct = new Entity(AnotherIri, Context.Object).ActLike<IProduct>();
        }
    }
}
