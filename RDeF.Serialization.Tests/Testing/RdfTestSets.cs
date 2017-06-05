using System.Collections.Generic;
using System.Linq;
using RDeF.Entities;
using RDeF.Vocabularies;

namespace RDeF.Testing
{
    internal class RdfTestSets
    {
        internal static readonly IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>> SimpleGraph =
            new[]
            {
                new Statement(new Iri("some:subject1"), new Iri("some:predicate1"), new Iri("some:object1")),
                new Statement(new Iri("some:subject1"), new Iri("some:predicate2"), new Iri("some:object2")),
                new Statement(new Iri("some:subject1"), new Iri("some:predicate1"), "value"),
                new Statement(new Iri("some:subject1"), new Iri("some:predicate2"), "value", xsd.@string),
                new Statement(new Iri("some:subject1"), new Iri("some:predicate1"), "123", xsd.@integer),
                new Statement(new Iri("some:subject1"), new Iri("some:predicate2"), "text", "en"),
                new Statement(new Iri("some:subject1"), rdf.type, new Iri("some:type1")),
                new Statement(new Iri("some:subject1"), rdf.type, new Iri("some:type2")),
                new Statement(new Iri("some:subject2"), new Iri("some:predicate1"), new Iri("some:object1")),
                new Statement(new Iri("some:subject2"), new Iri("some:predicate2"), new Iri("some:object2")),
                new Statement(new Iri("some:subject2"), new Iri("some:predicate1"), "value"),
                new Statement(new Iri("some:subject2"), new Iri("some:predicate2"), "value", xsd.@string),
                new Statement(new Iri("some:subject2"), new Iri("some:predicate1"), "123", xsd.@integer),
                new Statement(new Iri("some:subject2"), new Iri("some:predicate2"), "text", "en"),
                new Statement(new Iri("some:subject2"), rdf.type, new Iri("some:type1")),
                new Statement(new Iri("some:subject2"), rdf.type, new Iri("some:type2")),
                new Statement(new Iri("some:subject3"), new Iri("some:predicate1"), new Iri("some:object1"), new Iri("some:graph")),
                new Statement(new Iri("some:subject3"), new Iri("some:predicate2"), new Iri("some:object2"), new Iri("some:graph")),
                new Statement(new Iri("some:subject3"), new Iri("some:predicate1"), "value", null, new Iri("some:graph")),
                new Statement(new Iri("some:subject3"), new Iri("some:predicate2"), "value", xsd.@string, new Iri("some:graph")),
                new Statement(new Iri("some:subject3"), new Iri("some:predicate1"), "123", xsd.@integer, new Iri("some:graph")),
                new Statement(new Iri("some:subject3"), new Iri("some:predicate2"), "text", "en", new Iri("some:graph")),
                new Statement(new Iri("some:subject3"), rdf.type, new Iri("some:type1"), new Iri("some:graph")),
                new Statement(new Iri("some:subject3"), rdf.type, new Iri("some:type2"), new Iri("some:graph"))
            }.GroupBy(statement => statement.Graph).Select(group => new KeyValuePair<Iri, IEnumerable<Statement>>(group.Key, group));

        internal static readonly IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>> ComplexGraph =
            new[]
            {
                new Statement(new Iri("some:subject1"), new Iri("some:predicate1"), new Iri("_:blank001")),
                new Statement(new Iri("_:blank001"), rdf.first, "1", xsd.@integer),
                new Statement(new Iri("_:blank001"), rdf.rest, new Iri("_:blank002")),
                new Statement(new Iri("_:blank002"), rdf.first, "2", xsd.@integer),
                new Statement(new Iri("_:blank002"), rdf.rest, rdf.nil),
                new Statement(new Iri("some:subject1"), new Iri("some:predicate1"), new Iri("_:blank011")),
                new Statement(new Iri("_:blank011"), rdf.first, "1", xsd.@double),
                new Statement(new Iri("_:blank011"), rdf.rest, new Iri("_:blank012")),
                new Statement(new Iri("_:blank012"), rdf.first, "2.1", xsd.@double),
                new Statement(new Iri("_:blank012"), rdf.rest, rdf.nil)
            }.GroupBy(statement => statement.Graph).Select(group => new KeyValuePair<Iri, IEnumerable<Statement>>(group.Key, group));
    }
}
