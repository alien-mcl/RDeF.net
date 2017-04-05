using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using RDeF.Entities;
using RDeF.Serialization;
using RDeF.Vocabularies;

namespace RDeF.Testing
{
    public abstract class RdfWriterSerializationTest<T> : RdfWriterTest<T> where T : IRdfWriter, new()
    {
        protected string Expected { get; private set; }

        protected MemoryStream Stream { get; private set; }

        protected IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>> Graphs { get; set; }

        private string ExpectedSerializationResourceName { get; set; }

        public override void TheTest()
        {
            Writer.Write(new StreamWriter(Stream), Graphs).GetAwaiter().GetResult();
        }

        protected override void ScenarioSetup()
        {
            ExpectedSerializationResourceName = GetType().FullName.Replace(".", "\\");
            var extension = Regex.Match(ExpectedSerializationResourceName, "\\\\([A-Z][a-z]+)").Groups[1].Value.ToLower();
            ExpectedSerializationResourceName += "." + extension;
            Expected = new StreamReader(GetType().GetTypeInfo().Assembly.GetManifestResourceStream(ExpectedSerializationResourceName)).ReadToEnd();
            if (extension != "rdf")
            {
                Expected = Expected.Cleaned();
            }

            Stream = new MemoryStream();
        }

        protected void WithSimpleGraph()
        {
            Graphs = new[]
            {
                new Statement(new Iri("subject1"), new Iri("predicate1"), new Iri("object1")),
                new Statement(new Iri("subject1"), new Iri("predicate2"), new Iri("object2")),
                new Statement(new Iri("subject1"), new Iri("predicate1"), "value"),
                new Statement(new Iri("subject1"), new Iri("predicate2"), "value", xsd.@string),
                new Statement(new Iri("subject1"), new Iri("predicate1"), "123", xsd.@int),
                new Statement(new Iri("subject1"), new Iri("predicate2"), "text", "en"),
                new Statement(new Iri("subject1"), rdf.type, new Iri("type1")),
                new Statement(new Iri("subject1"), rdf.type, new Iri("type2")),
                new Statement(new Iri("subject2"), new Iri("predicate1"), new Iri("object1")),
                new Statement(new Iri("subject2"), new Iri("predicate2"), new Iri("object2")),
                new Statement(new Iri("subject2"), new Iri("predicate1"), "value"),
                new Statement(new Iri("subject2"), new Iri("predicate2"), "value", xsd.@string),
                new Statement(new Iri("subject2"), new Iri("predicate1"), "123", xsd.@int),
                new Statement(new Iri("subject2"), new Iri("predicate2"), "text", "en"),
                new Statement(new Iri("subject2"), rdf.type, new Iri("type1")),
                new Statement(new Iri("subject2"), rdf.type, new Iri("type2")),
                new Statement(new Iri("subject3"), new Iri("predicate1"), new Iri("object1"), new Iri("graph")),
                new Statement(new Iri("subject3"), new Iri("predicate2"), new Iri("object2"), new Iri("graph")),
                new Statement(new Iri("subject3"), new Iri("predicate1"), "value", (Iri)null, new Iri("graph")),
                new Statement(new Iri("subject3"), new Iri("predicate2"), "value", xsd.@string, new Iri("graph")),
                new Statement(new Iri("subject3"), new Iri("predicate1"), "123", xsd.@int, new Iri("graph")),
                new Statement(new Iri("subject3"), new Iri("predicate2"), "text", "en", new Iri("graph")),
                new Statement(new Iri("subject3"), rdf.type, new Iri("type1"), new Iri("graph")),
                new Statement(new Iri("subject3"), rdf.type, new Iri("type2"), new Iri("graph"))
            }.GroupBy(statement => statement.Graph).Select(group => new KeyValuePair<Iri, IEnumerable<Statement>>(group.Key, group));
        }

        protected void WithCollectionsGraph()
        {
            Graphs = new[]
            {
                new Statement(new Iri("subject1"), new Iri("predicate1"), new Iri("_:blank001")),
                new Statement(new Iri("_:blank001"), rdf.first, "1", xsd.@int),
                new Statement(new Iri("_:blank001"), rdf.rest, new Iri("_:blank002")),
                new Statement(new Iri("_:blank002"), rdf.first, "2", xsd.@int),
                new Statement(new Iri("_:blank002"), rdf.rest, rdf.nil),
                new Statement(new Iri("subject1"), new Iri("predicate1"), new Iri("_:blank011")),
                new Statement(new Iri("_:blank011"), rdf.first, "1", xsd.@double),
                new Statement(new Iri("_:blank011"), rdf.rest, new Iri("_:blank012")),
                new Statement(new Iri("_:blank012"), rdf.first, "2.1", xsd.@double),
                new Statement(new Iri("_:blank012"), rdf.rest, rdf.nil)
            }.GroupBy(statement => statement.Graph).Select(group => new KeyValuePair<Iri, IEnumerable<Statement>>(group.Key, group));
        }
    }
}
