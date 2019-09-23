using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using VDS.RDF;
using IGraph = RDeF.Serialization.IGraph;

namespace RDef.RDF
{
    internal class InMemoryTripleStore : ITripleStore
    {
        internal InMemoryTripleStore(IEnumerable<IGraph> graphs, bool concatenateAllGraphs = false)
        {
            Graphs = new GraphsCollection(graphs, concatenateAllGraphs);
        }

        /// <inheritdoc />
        public event TripleStoreEventHandler GraphAdded
        {
            add { }
            remove { }
        }

        /// <inheritdoc />
        public event TripleStoreEventHandler GraphRemoved
        {
            add { }
            remove { }
        }

        /// <inheritdoc />
        public event TripleStoreEventHandler GraphChanged
        {
            add { }
            remove { }
        }

        /// <inheritdoc />
        public event TripleStoreEventHandler GraphCleared
        {
            add { }
            remove { }
        }

        /// <inheritdoc />
        public event TripleStoreEventHandler GraphMerged
        {
            add { }
            remove { }
        }
        
        /// <inheritdoc />
        public bool IsEmpty
        {
            get { return !Graphs.Any(); }
        }

        /// <inheritdoc />
        public BaseGraphCollection Graphs { get; }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public IEnumerable<Triple> Triples
        {
            get { return Array.Empty<Triple>(); }
        }

        /// <inheritdoc />
        public VDS.RDF.IGraph this[Uri graphUri]
        {
            get { return null; }
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public bool HasGraph(Uri graphUri)
        {
            return Graphs.Contains(graphUri);
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", Justification = "Members are disposed correctly. This is an FxCop bug.")]
        public void Dispose()
        {
            Graphs.Dispose();
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public bool Add(VDS.RDF.IGraph g)
        {
            return false;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public bool Add(VDS.RDF.IGraph g, bool mergeIfExists)
        {
            return false;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public bool AddFromUri(Uri graphUri)
        {
            return false;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public bool AddFromUri(Uri graphUri, bool mergeIfExists)
        {
            return false;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public bool Remove(Uri graphUri)
        {
            return false;
        }
    }
}
