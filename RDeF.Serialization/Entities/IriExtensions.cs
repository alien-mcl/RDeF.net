using VDS.RDF;

namespace RDeF.Entities
{
    internal static class IriExtensions
    {
        internal static INode ToNode(this Iri iri, INodeFactory nodeFactory)
        {
            return iri.IsBlank ? (INode)nodeFactory.CreateBlankNode(iri.Id) : nodeFactory.CreateUriNode(iri);
        }
    }
}
