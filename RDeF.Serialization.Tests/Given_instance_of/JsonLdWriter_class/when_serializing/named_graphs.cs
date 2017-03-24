using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using RDeF;
using RDeF.Entities;
using RDeF.Vocabularies;

namespace Given_instance_of.JsonLdWriter_class.when_serializing
{
    [TestFixture]
    public class named_graphs : ScenarioTest
    {
        [Test]
        public void Should_serialize_to_Json_Ld_correctly()
        {
            Encoding.UTF8.GetString(Stream.ToArray()).Cleaned().Should().Be(Expected);
        }

        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();
            Graphs = new[]
            {
                new Statement(new Iri("subject1"), new Iri("predicate1"), new Iri("object1")),
                new Statement(new Iri("subject1"), new Iri("predicate2"), new Iri("object2")),
                new Statement(new Iri("subject1"), new Iri("predicate1"), "value"),
                new Statement(new Iri("subject1"), new Iri("predicate2"), "value", xsd.@string),
                new Statement(new Iri("subject1"), new Iri("predicate1"), "123", xsd.@int),
                new Statement(new Iri("subject1"), new Iri("predicate2"), "text", "en"),
                new Statement(new Iri("subject1"), rdfs.type, new Iri("type1")),
                new Statement(new Iri("subject1"), rdfs.type, new Iri("type2")),
                new Statement(new Iri("subject2"), new Iri("predicate1"), new Iri("object1")),
                new Statement(new Iri("subject2"), new Iri("predicate2"), new Iri("object2")),
                new Statement(new Iri("subject2"), new Iri("predicate1"), "value"),
                new Statement(new Iri("subject2"), new Iri("predicate2"), "value", xsd.@string),
                new Statement(new Iri("subject2"), new Iri("predicate1"), "123", xsd.@int),
                new Statement(new Iri("subject2"), new Iri("predicate2"), "text", "en"),
                new Statement(new Iri("subject2"), rdfs.type, new Iri("type1")),
                new Statement(new Iri("subject2"), rdfs.type, new Iri("type2")),
                new Statement(new Iri("subject3"), new Iri("predicate1"), new Iri("object1"), new Iri("graph")),
                new Statement(new Iri("subject3"), new Iri("predicate2"), new Iri("object2"), new Iri("graph")),
                new Statement(new Iri("subject3"), new Iri("predicate1"), "value", (Iri)null, new Iri("graph")),
                new Statement(new Iri("subject3"), new Iri("predicate2"), "value", xsd.@string, new Iri("graph")),
                new Statement(new Iri("subject3"), new Iri("predicate1"), "123", xsd.@int, new Iri("graph")),
                new Statement(new Iri("subject3"), new Iri("predicate2"), "text", "en", new Iri("graph")),
                new Statement(new Iri("subject3"), rdfs.type, new Iri("type1"), new Iri("graph")),
                new Statement(new Iri("subject3"), rdfs.type, new Iri("type2"), new Iri("graph"))
            }.GroupBy(statement => statement.Graph).Select(group => new KeyValuePair<Iri, IEnumerable<Statement>>(group.Key, group));
        }
    }
}
