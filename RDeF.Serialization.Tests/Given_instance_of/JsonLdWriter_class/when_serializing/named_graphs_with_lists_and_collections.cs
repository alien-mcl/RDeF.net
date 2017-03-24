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
    public class named_graphs_with_lists_and_collections : ScenarioTest
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
                new Statement(new Iri("subject1"), new Iri("predicate1"), new Iri("_:blank001")),
                new Statement(new Iri("_:blank001"), rdf.first, "1", xsd.@int),
                new Statement(new Iri("_:blank001"), rdf.last, new Iri("_:blank002")),
                new Statement(new Iri("_:blank002"), rdf.first, "2", xsd.@int),
                new Statement(new Iri("_:blank002"), rdf.last, rdf.nil),
                new Statement(new Iri("subject1"), new Iri("predicate1"), new Iri("_:blank011")),
                new Statement(new Iri("_:blank011"), rdf.first, "1", xsd.@double),
                new Statement(new Iri("_:blank011"), rdf.last, new Iri("_:blank012")),
                new Statement(new Iri("_:blank012"), rdf.first, "2.1", xsd.@double),
                new Statement(new Iri("_:blank012"), rdf.last, rdf.nil)
            }.GroupBy(statement => statement.Graph).Select(group => new KeyValuePair<Iri, IEnumerable<Statement>>(group.Key, group));
        }
    }
}
