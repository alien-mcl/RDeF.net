using System;
using JsonLD.Core;
using RDeF.Entities;

namespace RDeF.Serialization
{
    internal static class RDFDatasetExtensions
    {
        internal static Statement AsStatement(this RDFDataset.Quad quad)
        {
            var graph = (quad.ContainsKey("name") ? quad.GetGraph().AsIri() : null);
            var subject = quad.GetSubject().AsIri();
            var predicate = quad.GetPredicate().AsIri();
            if (quad.GetObject().IsIRI())
            {
                return new Statement(subject, predicate, quad.GetObject().AsIri(), graph);
            }

            if (!String.IsNullOrEmpty(quad.GetObject().GetLanguage()))
            {
                return new Statement(subject, predicate, quad.GetObject().GetValue(), quad.GetObject().GetLanguage(), graph);
            }

            var dataType = (quad.GetObject().GetDatatype() != null ? new Iri(quad.GetObject().GetDatatype()) : null);
            return new Statement(subject, predicate, quad.GetObject().GetValue(), dataType, graph);
        }

        private static Iri AsIri(this RDFDataset.Node node)
        {
            if (node == null)
            {
                return null;
            }

            return (node is RDFDataset.BlankNode ? new Iri() : new Iri(((RDFDataset.IRI)node).GetValue()));
        }
    }

}
