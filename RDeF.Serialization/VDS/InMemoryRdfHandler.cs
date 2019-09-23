using System;
using System.Collections.Generic;
using RDeF.Entities;
using RDeF.Serialization;
using VDS.RDF;
using VDS.RDF.Parsing.Handlers;

namespace RDef.RDF
{
    internal class InMemoryRdfHandler : BaseRdfHandler
    {
        internal InMemoryRdfHandler()
        {
        }

        /// <inheritdoc />
        public override bool AcceptsAll { get; } = true;

        internal IDictionary<Iri, WritableGraph> Graphs { get; } = new Dictionary<Iri, WritableGraph>();

        /// <inheritdoc />
        protected override bool HandleTripleInternal(Triple t)
        {
            bool result = false;
            if (t != null)
            {
                var graphIri = t.GraphUri == null ? Iri.DefaultGraph : (Iri)t.GraphUri;
                WritableGraph graph;
                if (!Graphs.TryGetValue(graphIri, out graph))
                {
                    Graphs[graphIri] = graph = new WritableGraph(graphIri);
                }

                var subject = t.Subject.ToIri();
                var predicate = t.Predicate.ToIri();
                Statement statement;
                var value = t.Object as ILiteralNode;
                if (value != null)
                {
                    statement = !String.IsNullOrEmpty(value.Language)
                        ? new Statement(subject, predicate, value.Value, value.Language, graphIri)
                        : new Statement(subject, predicate, value.Value, value.DataType, graphIri);
                }
                else
                {
                    statement = new Statement(subject, predicate, t.Object.ToIri(), graphIri);
                }

                graph.Statements.Add(statement);
                result = true;
            }

            return result;
        }
    }
}
