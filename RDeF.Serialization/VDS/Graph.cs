using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using RDeF.Entities;
using VDS.RDF;
using IGraph = RDeF.Serialization.IGraph;

namespace RDef.RDF
{
    internal class Graph : VDS.RDF.IGraph
    {
        private readonly IGraph _graph;

        internal Graph(IGraph graph)
        {
            _graph = graph;
        }
        
        /// <inheritdoc />
        public event TripleEventHandler TripleAsserted
        {
            add { }
            remove { }
        }

        /// <inheritdoc />
        public event TripleEventHandler TripleRetracted
        {
            add { }
            remove { }
        }

        /// <inheritdoc />
        public event GraphEventHandler Changed
        {
            add { }
            remove { }
        }

        /// <inheritdoc />
        public event CancellableGraphEventHandler ClearRequested
        {
            add { }
            remove { }
        }

        /// <inheritdoc />
        public event GraphEventHandler Cleared
        {
            add { }
            remove { }
        }

        /// <inheritdoc />
        public event CancellableGraphEventHandler MergeRequested
        {
            add { }
            remove { }
        }

        /// <inheritdoc />
        public event GraphEventHandler Merged
        {
            add { }
            remove { }
        }

        /// <inheritdoc />
        public Uri BaseUri
        {
            get { return _graph.Iri == Iri.DefaultGraph ? null : _graph.Iri; }
            set { }
        }

        /// <inheritdoc />
        public bool IsEmpty
        {
            get { return !_graph.Statements.Any(); }
        }

        /// <inheritdoc />
        public INamespaceMapper NamespaceMap
        {
            get { return null; }
        }

        /// <inheritdoc />
        public IEnumerable<INode> Nodes
        {
            get { return Array.Empty<INode>(); }
        }

        /// <inheritdoc />
        public BaseTripleCollection Triples
        {
            get { return new RDeF.RDF.TripleCollection(_graph.Statements); }
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public IBlankNode CreateBlankNode()
        {
            return null;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public IBlankNode CreateBlankNode(string nodeId)
        {
            return null;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public IGraphLiteralNode CreateGraphLiteralNode()
        {
            return null;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public IGraphLiteralNode CreateGraphLiteralNode(VDS.RDF.IGraph subgraph)
        {
            return null;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public ILiteralNode CreateLiteralNode(string literal, Uri datatype)
        {
            return null;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public ILiteralNode CreateLiteralNode(string literal)
        {
            return null;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public ILiteralNode CreateLiteralNode(string literal, string langspec)
        {
            return null;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public IUriNode CreateUriNode(Uri uri)
        {
            return null;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public IVariableNode CreateVariableNode(string varname)
        {
            return null;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public string GetNextBlankNodeID()
        {
            return null;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            //// Nothing to dipose.
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public void ReadXml(XmlReader reader)
        {
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public void WriteXml(XmlWriter writer)
        {
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public bool Assert(Triple t)
        {
            return false;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public bool Assert(IEnumerable<Triple> ts)
        {
            return false;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public bool Retract(Triple t)
        {
            return false;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public bool Retract(IEnumerable<Triple> ts)
        {
            return false;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public void Clear()
        {
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public IUriNode CreateUriNode()
        {
            return null;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public IUriNode CreateUriNode(string qname)
        {
            return null;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public IBlankNode GetBlankNode(string nodeId)
        {
            return null;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public ILiteralNode GetLiteralNode(string literal, string langspec)
        {
            return null;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public ILiteralNode GetLiteralNode(string literal)
        {
            return null;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public ILiteralNode GetLiteralNode(string literal, Uri datatype)
        {
            return null;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public IUriNode GetUriNode(string qname)
        {
            return null;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public IUriNode GetUriNode(Uri uri)
        {
            return null;
        }
        
        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public IEnumerable<Triple> GetTriples(Uri uri)
        {
            return Array.Empty<Triple>();
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public IEnumerable<Triple> GetTriples(INode n)
        {
            return Array.Empty<Triple>();
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public IEnumerable<Triple> GetTriplesWithObject(Uri u)
        {
            return Array.Empty<Triple>();
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public IEnumerable<Triple> GetTriplesWithObject(INode n)
        {
            return Array.Empty<Triple>();
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public IEnumerable<Triple> GetTriplesWithPredicate(INode n)
        {
            return Array.Empty<Triple>();
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public IEnumerable<Triple> GetTriplesWithPredicate(Uri u)
        {
            return Array.Empty<Triple>();
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public IEnumerable<Triple> GetTriplesWithSubject(INode n)
        {
            return Array.Empty<Triple>();
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public IEnumerable<Triple> GetTriplesWithSubject(Uri u)
        {
            return Array.Empty<Triple>();
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public IEnumerable<Triple> GetTriplesWithSubjectPredicate(INode subj, INode pred)
        {
            return Array.Empty<Triple>();
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public IEnumerable<Triple> GetTriplesWithSubjectObject(INode subj, INode obj)
        {
            return Array.Empty<Triple>();
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public IEnumerable<Triple> GetTriplesWithPredicateObject(INode pred, INode obj)
        {
            return Array.Empty<Triple>();
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public bool ContainsTriple(Triple t)
        {
            return false;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public void Merge(VDS.RDF.IGraph g)
        {
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public void Merge(VDS.RDF.IGraph g, bool keepOriginalGraphUri)
        {
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public bool Equals(VDS.RDF.IGraph g, out Dictionary<INode, INode> mapping)
        {
            mapping = null;
            return false;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public bool IsSubGraphOf(VDS.RDF.IGraph g)
        {
            return false;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public bool IsSubGraphOf(VDS.RDF.IGraph g, out Dictionary<INode, INode> mapping)
        {
            mapping = null;
            return false;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public bool HasSubGraph(VDS.RDF.IGraph g)
        {
            return false;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public bool HasSubGraph(VDS.RDF.IGraph g, out Dictionary<INode, INode> mapping)
        {
            mapping = null;
            return false;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public GraphDiffReport Difference(VDS.RDF.IGraph g)
        {
            return null;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public Uri ResolveQName(string qname)
        {
            return null;
        }
    }
}
