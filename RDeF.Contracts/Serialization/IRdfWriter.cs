using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace RDeF.Serialization
{
    /// <summary>Describes an abstract RDF writing facility.</summary>
    public interface IRdfWriter
    {
        /// <summary>Writes an RDF <paramref name="graphs"/> into a given <paramref name="streamWriter" />.</summary>
        /// <param name="streamWriter">Stream writer into which RDF data should be written.</param>
        /// <param name="graphs">Named graphs with RDF statements to be written.</param>
        /// <returns>Task of this operation.</returns>
        Task Write(StreamWriter streamWriter, IEnumerable<IGraph> graphs);

        /// <summary>Writes an RDF <paramref name="graphs"/> into a given <paramref name="streamWriter" />.</summary>
        /// <param name="streamWriter">Stream writer into which RDF data should be written.</param>
        /// <param name="graphs">Named graphs with RDF statements to be written.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task of this operation.</returns>
        Task Write(StreamWriter streamWriter, IEnumerable<IGraph> graphs, CancellationToken cancellationToken);
    }
}
