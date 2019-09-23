using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RDef.RDF;
using VDS.RDF;

namespace RDeF.Serialization
{
    /// <summary>Writes RDF data.</summary>
    public abstract class RdfWriterBase : IRdfWriter
    {
        /// <summary>Gets a value indicating whether the reader supports RDF graphs.</summary>
        protected abstract bool SupportsGraphs { get; }

        /// <inheritdoc />
        public Task Write(StreamWriter streamWriter, IEnumerable<IGraph> graphs)
        {
            return Write(streamWriter, graphs, CancellationToken.None);
        }

        /// <inheritdoc />
        public Task Write(StreamWriter streamWriter, IEnumerable<IGraph> graphs, CancellationToken cancellationToken)
        {
            using (var store = new InMemoryTripleStore(graphs, !SupportsGraphs))
            {
                if (SupportsGraphs)
                {
                    CreateStoreWriter().Save(store, streamWriter, true);
                }
                else
                {
                    CreateRdfWriter().Save(store.Graphs.First(), streamWriter, true);
                }
            }

            return Task.CompletedTask;
        }

        /// <summary>Creates an instance of the underlying RDF writer.</summary>
        /// <returns>RDF reader.</returns>
        protected abstract IStoreWriter CreateStoreWriter();

        /// <summary>Creates an instance of the underlying RDF writer.</summary>
        /// <returns>RDF reader.</returns>
        protected abstract global::VDS.RDF.IRdfWriter CreateRdfWriter();
    }
}
