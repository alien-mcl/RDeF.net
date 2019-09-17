using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using RDeF.Serialization;

namespace RDeF.Entities
{
    /// <summary>Describes an abstract <see cref="IEntitySource" /> that is serializable.</summary>
    public interface ISerializableEntitySource : IReadableEntitySource
    {
        /// <summary>Gets all statements from the entity store.</summary>
        IEnumerable<Statement> Statements { get; }

        /// <summary>Writes the content of the entity source into a given <paramref name="rdfWriter" />.</summary>
        /// <param name="streamWriter">Target stream writer.</param>
        /// <param name="rdfWriter">Target RDF writer.</param>
        /// <returns>Task of this operation.</returns>
        Task Write(StreamWriter streamWriter, IRdfWriter rdfWriter);

        /// <summary>Writes the content of the entity source into a given <paramref name="rdfWriter" />.</summary>
        /// <param name="streamWriter">Target stream writer.</param>
        /// <param name="rdfWriter">Target RDF writer.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task of this operation.</returns>
        Task Write(StreamWriter streamWriter, IRdfWriter rdfWriter, CancellationToken cancellationToken);

        /// <summary>Reads new statements from a given <paramref name="streamReader" /> using a given <paramref name="rdfReader" />.</summary>
        /// <param name="streamReader">Source stream of RDF data.</param>
        /// <param name="rdfReader">Target RDF reading facility.</param>
        /// <returns>Task of this operation.</returns>
        Task Read(StreamReader streamReader, IRdfReader rdfReader);
        
        /// <summary>Reads new statements from a given <paramref name="streamReader" /> using a given <paramref name="rdfReader" />.</summary>
        /// <param name="streamReader">Source stream of RDF data.</param>
        /// <param name="rdfReader">Target RDF reading facility.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task of this operation.</returns>
        Task Read(StreamReader streamReader, IRdfReader rdfReader, CancellationToken cancellationToken);
    }
}
