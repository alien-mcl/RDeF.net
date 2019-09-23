using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using RDeF.Entities;
using VDS.RDF;
using IGraph = RDeF.Serialization.IGraph;

namespace RDef.RDF
{
    internal class GraphsCollection : BaseGraphCollection
    {
        private readonly IEnumerable<IGraph> _graphs;
        private readonly bool _concatenateAllGraphs;

        internal GraphsCollection(IEnumerable<IGraph> graphs, bool concatenateAllGraphs)
        {
            _graphs = graphs;
            _concatenateAllGraphs = concatenateAllGraphs;
        }

        /// <inheritdoc />
        public override int Count
        {
            get { return _concatenateAllGraphs && _graphs.Any() ? 1 : _graphs.Count(); }
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public override IEnumerable<Uri> GraphUris
        {
            get
            {
                return _concatenateAllGraphs ? new Uri[] { null } : _graphs.Select(_ => _.Iri == Iri.DefaultGraph ? null : (Uri)_.Iri);
            }
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public override VDS.RDF.IGraph this[Uri graphUri]
        {
            get { return null; }
        }
                
        /// <inheritdoc />
        public override IEnumerator<VDS.RDF.IGraph> GetEnumerator()
        {
            return _concatenateAllGraphs
                ? new List<VDS.RDF.IGraph>() { new ConcatenatedGraph(_graphs) }.GetEnumerator()
                : _graphs.Select(_ => (VDS.RDF.IGraph)new Graph(_)).GetEnumerator();
        }

        /// <inheritdoc />
        public override bool Contains(Uri graphUri)
        {
            return _graphs.Any(_ => (graphUri == null && _.Iri == Iri.DefaultGraph) || (_.Iri == (Iri)graphUri));
        }
        
        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public override void Dispose()
        {
            //// Nothing to dispose.
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        protected override bool Add(global::VDS.RDF.IGraph g, bool mergeIfExists)
        {
            return false;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        protected override bool Remove(Uri graphUri)
        {
            return false;
        }
    }
}
