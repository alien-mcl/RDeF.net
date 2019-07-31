using System;
using JsonLD.Core;
using RDeF.Entities;

namespace RDeF.Serialization
{
    /// <summary>Provides useful <see cref="RDFDataset.Quad" /> extensions.</summary>
    public static class QuadExtensions
    {
        /// <summary>Transforms a given <paramref name="quad" /> into a <see cref="Statement" />.</summary>
        /// <param name="quad">Quad to be transformed.</param>
        /// <returns>Statement being an equivalent of the given <paramref name="quad" />.</returns>
        public static Statement AsStatement(this RDFDataset.Quad quad)
        {
            Statement result = null;
            if (quad != null)
            {
                var graph = (quad.ContainsKey("name") ? quad.GetGraph().AsIri() : null);
                var subject = quad.GetSubject().AsIri();
                var predicate = quad.GetPredicate().AsIri();
                if (quad.GetObject().IsIRI() || quad.GetObject().IsBlankNode())
                {
                    return new Statement(subject, predicate, quad.GetObject().AsIri(), graph);
                }

                if (!String.IsNullOrEmpty(quad.GetObject().GetLanguage()))
                {
                    return new Statement(subject, predicate, quad.GetObject().GetValue(), quad.GetObject().GetLanguage(), graph);
                }

                var dataType = (quad.GetObject().GetDatatype() != null ? new Iri(quad.GetObject().GetDatatype()) : null);
                result = new Statement(subject, predicate, quad.GetObject().GetValue(), dataType, graph);
            }

            return result;
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
