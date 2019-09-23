using RDeF.Entities;
using VDS.RDF;

namespace RDef.RDF
{
    internal static class NodeExtensions
    {
        internal static Iri ToIri(this INode node)
        {
            var blankNode = node as IBlankNode;
            return blankNode != null ? new Iri(blankNode.InternalID) : new Iri(((UriNode)node).Uri);
        }
    }
}
