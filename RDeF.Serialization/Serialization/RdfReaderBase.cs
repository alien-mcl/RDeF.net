using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using RDef.RDF;
using VDS.RDF;

namespace RDeF.Serialization
{
    /// <summary>Allows to read RDF graphs.</summary>
    public abstract class RdfReaderBase : IRdfReader
    {
        /// <summary>Gets a value indicating whether the reader supports RDF graphs.</summary>
        protected abstract bool SupportsGraphs { get; }

        /// <summary>Reads an RDF graph from a given <paramref name="streamReader" />.</summary>
        /// <param name="streamReader">Stream reader from which RDF data should be read.</param>
        /// <param name="baseUri">Optional base <see cref="Uri" />.</param>
        /// <returns>Map of resources and their statements.</returns>
        [SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", Justification = "Searched string is culture invariant.")]
        public Task<IEnumerable<IGraph>> Read(StreamReader streamReader, Uri baseUri = null)
        {
            return Read(streamReader, baseUri, CancellationToken.None);
        }

        /// <summary>Reads an RDF graph from a given <paramref name="streamReader" />.</summary>
        /// <param name="streamReader">Stream reader from which RDF data should be read.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Map of resources and their statements.</returns>
        [SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", Justification = "Searched string is culture invariant.")]
        public Task<IEnumerable<IGraph>> Read(StreamReader streamReader, CancellationToken cancellationToken)
        {
            return Read(streamReader, null, cancellationToken);
        }

        /// <summary>Reads an RDF graph from a given <paramref name="streamReader" />.</summary>
        /// <param name="streamReader">Stream reader from which RDF data should be read.</param>
        /// <param name="baseUri">Optional base <see cref="Uri" />.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Map of resources and their statements.</returns>
        [SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", Justification = "Searched string is culture invariant.")]
        public Task<IEnumerable<IGraph>> Read(StreamReader streamReader, Uri baseUri, CancellationToken cancellationToken)
        {
            if (streamReader == null)
            {
                throw new ArgumentNullException(nameof(streamReader));
            }

            var buffer = new InMemoryRdfHandler();
            if (SupportsGraphs)
            {
                var reader = CreateStoreReader(baseUri);
                reader.Load(buffer, streamReader);
            }
            else
            {
                var reader = CreateRdfReader(baseUri);
                reader.Load(buffer, streamReader);
            }

            return Task.FromResult<IEnumerable<IGraph>>(buffer.Graphs.Values);
        }

        /// <summary>Creates an instance of the underlying RDF reader.</summary>
        /// <param name="baseUri">Base <see cref="Uri" /> to be used by the reader for resolving relative <see cref="Uri" />.</param>
        /// <returns>RDF reader.</returns>
        protected abstract IStoreReader CreateStoreReader(Uri baseUri);

        /// <summary>Creates an instance of the underlying RDF reader.</summary>
        /// <param name="baseUri">Base <see cref="Uri" /> to be used by the reader for resolving relative <see cref="Uri" />.</param>
        /// <returns>RDF reader.</returns>
        protected abstract global::VDS.RDF.IRdfReader CreateRdfReader(Uri baseUri);
    }
}
