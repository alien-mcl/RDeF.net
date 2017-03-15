using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Entities;
using RDeF.Vocabularies;

namespace Given_instance_of.JsonLdWriter_class
{
    [TestFixture]
    public class when_serializing_named_graphs : JsonLdWriterTest
    {
        private static readonly string ExpectedSerializationResourceName = typeof(when_serializing_named_graphs).FullName.Replace(".", "\\") + ".json";
        private static readonly string Expected = Cleaned(new StreamReader(typeof(JsonLdWriterTest).Assembly.GetManifestResourceStream(ExpectedSerializationResourceName)).ReadToEnd());

        private MemoryStream Stream { get; set; }

        private IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>> Graphs { get; set; }

        public override void TheTest()
        {
            Writer.Write(new StreamWriter(Stream), Graphs);
        }

        [Test]
        public void Should_serialize_to_Json_Ld_correctly()
        {
            Cleaned(Encoding.UTF8.GetString(Stream.ToArray())).Should().Be(Expected);
        }

        protected override void ScenarioSetup()
        {
            Stream = new MemoryStream();
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
                new Statement(new Iri("subject3"), rdfs.type, new Iri("type2"), new Iri("graph")),
            }.GroupBy(statement => statement.Graph).Select(group => new KeyValuePair<Iri, IEnumerable<Statement>>(group.Key, group));
        }
    }
}
