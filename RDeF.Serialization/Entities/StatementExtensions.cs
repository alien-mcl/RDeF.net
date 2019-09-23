using System;
using VDS.RDF;

namespace RDeF.Entities
{
    internal static class StatementExtensions
    {
        internal static Triple ToTriple(this Statement statement, INodeFactory nodeFactory)
        {
            var subject = statement.Subject.ToNode(nodeFactory);
            var predicate = statement.Predicate.ToNode(nodeFactory);
            var graphIri = statement.Graph == Iri.DefaultGraph ? null : (Uri)statement.Graph;
            if (statement.Object != null)
            {
                return new Triple(subject, predicate, statement.Object.ToNode(nodeFactory), graphIri);
            }

            if (!String.IsNullOrEmpty(statement.Language))
            {
                return new Triple(subject, predicate, nodeFactory.CreateLiteralNode(statement.Value, statement.Language), graphIri);
            }

            return new Triple(subject, predicate, nodeFactory.CreateLiteralNode(statement.Value, (Uri)statement.DataType), graphIri);
        }
    }
}
