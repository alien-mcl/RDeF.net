using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using RDeF.Entities;

namespace RDeF.Serialization
{
    /// <summary>Describes an abstract RDF reading facility.</summary>
    public interface IRdfReader
    {
        /// <summary>Reads an RDF graph from a given <paramref name="streamReader" />.</summary>
        /// <param name="streamReader">Stream reader from which RDF data should be read.</param>
        /// <returns>Map of resources and their statements.</returns>
        Task<IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>>> Read(StreamReader streamReader);

        /// <summary>Reads an RDF graph from a given <paramref name="streamReader" />.</summary>
        /// <param name="streamReader">Stream reader from which RDF data should be read.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Map of resources and their statements.</returns>
        Task<IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>>> Read(StreamReader streamReader, CancellationToken cancellationToken);
    }
}
