using System.Collections.Generic;
using System.IO;
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
    }
}
