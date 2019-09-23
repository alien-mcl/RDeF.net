using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace RDeF.Serialization
{
    /// <summary>Describes an abstract RDF reading facility.</summary>
    public interface IRdfReader
    {
        /// <summary>Reads an RDF graph from a given <paramref name="streamReader" />.</summary>
        /// <param name="streamReader">Stream reader from which RDF data should be read.</param>
        /// <param name="baseUri">Optional base <see cref="Uri" />.</param>
        /// <returns>Graph of resources and their statements.</returns>
        Task<IEnumerable<IGraph>> Read(StreamReader streamReader, Uri baseUri = null);

        /// <summary>Reads an RDF graph from a given <paramref name="streamReader" />.</summary>
        /// <param name="streamReader">Stream reader from which RDF data should be read.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Graph of resources and their statements.</returns>
        Task<IEnumerable<IGraph>> Read(StreamReader streamReader, CancellationToken cancellationToken);

        /// <summary>Reads an RDF graph from a given <paramref name="streamReader" />.</summary>
        /// <param name="streamReader">Stream reader from which RDF data should be read.</param>
        /// <param name="baseUri">Optional base <see cref="Uri" />.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Graph of resources and their statements.</returns>
        Task<IEnumerable<IGraph>> Read(StreamReader streamReader, Uri baseUri, CancellationToken cancellationToken);
    }
}
