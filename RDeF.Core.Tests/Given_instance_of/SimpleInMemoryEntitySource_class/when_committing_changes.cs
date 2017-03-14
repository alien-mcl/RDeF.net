using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Entities;
using RDeF.Mapping;
using RDeF.Vocabularies;

namespace Given_instance_of.SimpleInMemoryEntitySource_class
{
    [TestFixture]
    public class when_committing_changes : SimpleInMemoryEntitySourceTest
    {
        private static readonly Iri Iri1 = new Iri("test1");
        private static readonly Iri Iri2 = new Iri("test2");
        private static readonly Statement TypeAssertion = new Statement(Iri1, rdfs.type, new Iri("class"));
        private static readonly Statement Property1Assertion = new Statement(Iri1, rdf.first, "1");
        private static readonly Statement Property2Assertion = new Statement(Iri1, rdf.last, "1");

        private IDictionary<IEntity, ISet<Statement>> RetractedStatements { get; set; }

        private IDictionary<IEntity, ISet<Statement>> AddedStatements { get; set; }

        private IEntity Entity1 { get; set; }

        private IEntity Entity2 { get; set; }

        public override void TheTest()
        {
            EntitySource.Commit(RetractedStatements, AddedStatements);
        }

        [Test]
        public void Should_remove_entity_without_any_statements()
        {
            EntitySource.Entities.Should().NotContainKey(Entity2);
        }

        [Test]
        public void Should_remove_retracted_statements()
        {
            EntitySource.Entities[Entity1].Should().NotContain(TypeAssertion);
        }

        [Test]
        public void Should_add_new_statements()
        {
            EntitySource.Entities[Entity1].Should().BeEquivalentTo(Property1Assertion, Property2Assertion);
        }

        protected override void ScenarioSetup()
        {
            var entityContext = new DefaultEntityContext(EntitySource, new Mock<IMappingsRepository>(MockBehavior.Strict).Object, new Mock<IChangeDetector>(MockBehavior.Strict).Object);
            Entity1 = new Entity(Iri1, entityContext);
            EntitySource.Entities[Entity1] = new HashSet<Statement>() { new Statement(Iri1, rdfs.type, new Iri("class")) };
            Entity2 = new Entity(Iri2, entityContext);
            EntitySource.Entities[Entity2] = new HashSet<Statement>() { new Statement(Iri2, rdfs.type, new Iri("class")) };
            RetractedStatements = new Dictionary<IEntity, ISet<Statement>>()
            {
                { Entity1, new HashSet<Statement>() { TypeAssertion } },
                { Entity2, new HashSet<Statement>() { new Statement(Iri2, rdfs.type, new Iri("class")) } }
            };
            AddedStatements = new Dictionary<IEntity, ISet<Statement>>()
            {
                { Entity1, new HashSet<Statement>() { Property1Assertion, Property2Assertion } }
            };
        }
    }
}
