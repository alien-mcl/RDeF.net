﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Entities;
using RDeF.Vocabularies;

namespace Given_instance_of.SimpleInMemoryEntitySource_class
{
    [TestFixture]
    public class when_committing_changes : SimpleInMemoryEntitySourceTest
    {
        private static readonly Iri Iri1 = new Iri("test1");
        private static readonly Iri Iri2 = new Iri("test2");
        private static readonly Iri Iri3 = new Iri("test3");
        private static readonly Statement TypeAssertion = new Statement(Iri1, rdf.type, new Iri("class"));
        private static readonly Statement Property1Assertion = new Statement(Iri1, rdf.first, "1");
        private static readonly Statement Property2Assertion = new Statement(Iri1, rdf.rest, "1");

        private ICollection<Iri> DeletedEntities { get; set; }

        private IDictionary<IEntity, ISet<Statement>> RetractedStatements { get; set; }

        private IDictionary<IEntity, ISet<Statement>> AddedStatements { get; set; }

        private IEntity Entity1 { get; set; }

        private IEntity Entity2 { get; set; }

        private IEntity Entity3 { get; set; }

        public override Task TheTest()
        {
            return EntitySource.Commit(DeletedEntities, RetractedStatements, AddedStatements);
        }

        [Test]
        public void Should_throw_when_no_deleted_entities_are_given()
        {
            EntitySource.Invoking(instance => instance.Commit(null, null, null)).ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("deletedEntities");
        }

        [Test]
        public void Should_throw_when_no_retracted_statements_are_given()
        {
            EntitySource.Invoking(instance => instance.Commit(DeletedEntities, null, null)).ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("retractedStatements");
        }

        [Test]
        public void Should_throw_when_no_added_statements_are_given()
        {
            EntitySource.Invoking(instance => instance.Commit(DeletedEntities, RetractedStatements, null))
                .ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("addedStatements");
        }

        [Test]
        public void Should_not_remove_entity_without_any_statements()
        {
            EntitySource.Entities.Should().ContainKey(Entity2);
        }

        [Test]
        public void Should_not_completely_remove_entity_without_any_statements()
        {
            EntitySource.EntityMap.Should().ContainKey(Entity2.Iri);
        }

        [Test]
        public void Should_remove_retracted_statements()
        {
            EntitySource.Entities[Entity1].Should().NotContain(TypeAssertion);
        }

        [Test]
        public void Should_remove_retracted_statements_without_removing_entity()
        {
            EntitySource.EntityMap.Should().ContainKey(Entity1.Iri);
        }

        [Test]
        public void Should_add_new_statements()
        {
            EntitySource.Entities[Entity1].Should().BeEquivalentTo(Property1Assertion, Property2Assertion);
        }

        [Test]
        public void Should_delete_deleted_entities()
        {
            EntitySource.Entities.Should().NotContainKey(Entity3);
        }

        [Test]
        public void Should_completely_delete_deleted_entities()
        {
            EntitySource.EntityMap.Should().NotContainKey(Entity3.Iri);
        }

        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();
            Entity1 = new Entity(Iri1, Context.Object);
            EntitySource.Entities[EntitySource.EntityMap[Entity1.Iri] = Entity1] = new HashSet<Statement>() { new Statement(Iri1, rdf.type, new Iri("class")) };
            Entity2 = new Entity(Iri2, Context.Object);
            EntitySource.Entities[EntitySource.EntityMap[Entity2.Iri] = Entity2] = new HashSet<Statement>() { new Statement(Iri2, rdf.type, new Iri("class")) };
            Entity3 = new Entity(Iri3, Context.Object);
            EntitySource.Entities[EntitySource.EntityMap[Entity3.Iri] = Entity3] = new HashSet<Statement>() { new Statement(Iri3, rdf.type, new Iri("class")) };
            DeletedEntities = new List<Iri>() { Iri3 };
            RetractedStatements = new Dictionary<IEntity, ISet<Statement>>()
            {
                { Entity1, new HashSet<Statement>() { TypeAssertion } },
                { Entity2, new HashSet<Statement>() { new Statement(Iri2, rdf.type, new Iri("class")) } }
            };
            AddedStatements = new Dictionary<IEntity, ISet<Statement>>()
            {
                { Entity1, new HashSet<Statement>() { Property1Assertion, Property2Assertion } }
            };
        }
    }
}
