using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Entities;

namespace Given_instance_of.SimpleInMemoryEntitySource_class
{
    [TestFixture]
    public class when_loading_an_entity : SimpleInMemoryEntitySourceTest
    {
        private ISet<Statement> ExpectedStatements { get; set; }

        [Test]
        public void Should_provide_entity_statements()
        {
            EntitySource.Load(new Iri("test")).Result.As<object>().Should().Be(ExpectedStatements);
        }

        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();
            var entity = new Entity(new Iri("test"), Context.Object);
            EntitySource.Entities[EntitySource.EntityMap[entity.Iri] = entity] = ExpectedStatements = new HashSet<Statement>()
            {
                new Statement(entity.Iri, new Iri("predicate"), entity.Iri),
                new Statement(entity.Iri, new Iri("predicate"), entity.Iri)
            };
        }
    }
}
