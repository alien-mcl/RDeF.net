using System.Collections.Generic;
using System.Linq;
using RDeF.Entities;
using RDeF.Serialization;
using RDeF.Vocabularies;

namespace RDeF.Testing
{
    internal class RdfTestSets
    {
        internal static readonly IEnumerable<IGraph> SimpleGraph =
            new[]
            {
                new Statement(new Iri("http://some/vocab#subject1"), new Iri("http://some/vocab#predicate1"), new Iri("http://some/vocab#object1")),
                new Statement(new Iri("http://some/vocab#subject1"), new Iri("http://some/vocab#predicate2"), new Iri("http://some/vocab#object2")),
                new Statement(new Iri("http://some/vocab#subject1"), new Iri("http://some/vocab#predicate1"), new Iri("_:b0")),
                new Statement(new Iri("http://some/vocab#subject1"), new Iri("http://some/vocab#predicate1"), "value"),
                new Statement(new Iri("http://some/vocab#subject1"), new Iri("http://some/vocab#predicate2"), "value", xsd.@string),
                new Statement(new Iri("http://some/vocab#subject1"), new Iri("http://some/vocab#predicate1"), "123", xsd.integer),
                new Statement(new Iri("http://some/vocab#subject1"), new Iri("http://some/vocab#predicate2"), "text", "en"),
                new Statement(new Iri("http://some/vocab#subject1"), rdf.type, new Iri("http://some/vocab#type1")),
                new Statement(new Iri("http://some/vocab#subject1"), rdf.type, new Iri("http://some/vocab#type2")),
                new Statement(new Iri("http://some/vocab#subject2"), new Iri("http://some/vocab#predicate1"), new Iri("http://some/vocab#object1")),
                new Statement(new Iri("http://some/vocab#subject2"), new Iri("http://some/vocab#predicate2"), new Iri("http://some/vocab#object2")),
                new Statement(new Iri("http://some/vocab#subject2"), new Iri("http://some/vocab#predicate1"), "value"),
                new Statement(new Iri("http://some/vocab#subject2"), new Iri("http://some/vocab#predicate2"), "value", xsd.@string),
                new Statement(new Iri("http://some/vocab#subject2"), new Iri("http://some/vocab#predicate1"), "123", xsd.integer),
                new Statement(new Iri("http://some/vocab#subject2"), new Iri("http://some/vocab#predicate2"), "text", "en"),
                new Statement(new Iri("http://some/vocab#subject2"), rdf.type, new Iri("http://some/vocab#type1")),
                new Statement(new Iri("http://some/vocab#subject2"), rdf.type, new Iri("http://some/vocab#type2")),
                new Statement(new Iri("http://some/vocab#subject3/"), new Iri("http://some/vocab#predicate1"), new Iri("http://some/vocab#object1"), new Iri("http://graph/")),
                new Statement(new Iri("http://some/vocab#subject3/"), new Iri("http://some/vocab#predicate2"), new Iri("http://some/vocab#object2"), new Iri("http://graph/")),
                new Statement(new Iri("http://some/vocab#subject3/"), new Iri("http://some/vocab#predicate1"), "value", null, new Iri("http://some/vocab#graph/")),
                new Statement(new Iri("http://some/vocab#subject3/"), new Iri("http://some/vocab#predicate2"), "value", xsd.@string, new Iri("http://some/vocab#graph/")),
                new Statement(new Iri("http://some/vocab#subject3/"), new Iri("http://some/vocab#predicate1"), "123", xsd.integer, new Iri("http://some/vocab#graph/")),
                new Statement(new Iri("http://some/vocab#subject3/"), new Iri("http://some/vocab#predicate2"), "text", "en", new Iri("http://some/vocab#graph/")),
                new Statement(new Iri("http://some/vocab#subject3/"), rdf.type, new Iri("http://some/vocab#type1"), new Iri("http://some/vocab#graph/")),
                new Statement(new Iri("http://some/vocab#subject3/"), rdf.type, new Iri("http://some/vocab#type2"), new Iri("http://some/vocab#graph/"))
            }.GroupBy(statement => statement.Graph).Select(group => new Graph(group.Key, group));

        internal static readonly IEnumerable<IGraph> ComplexGraph =
            new[]
            {
                new Statement(new Iri("http://some/vocab#subject1"), new Iri("http://some/vocab#predicate1"), new Iri("_:blank001")),
                new Statement(new Iri("_:blank001"), rdf.first, "1", xsd.integer),
                new Statement(new Iri("_:blank001"), rdf.rest, new Iri("_:blank002")),
                new Statement(new Iri("_:blank002"), rdf.first, "2", xsd.integer),
                new Statement(new Iri("_:blank002"), rdf.rest, rdf.nil),
                new Statement(new Iri("http://some/vocab#subject1"), new Iri("http://some/vocab#predicate1"), new Iri("_:blank011")),
                new Statement(new Iri("_:blank011"), rdf.first, "1", xsd.@double),
                new Statement(new Iri("_:blank011"), rdf.rest, new Iri("_:blank012")),
                new Statement(new Iri("_:blank012"), rdf.first, "2.1", xsd.@double),
                new Statement(new Iri("_:blank012"), rdf.rest, rdf.nil)
            }.GroupBy(statement => statement.Graph).Select(group => new Graph(group.Key, group));
    }
}
