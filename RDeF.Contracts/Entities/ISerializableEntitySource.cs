using System.IO;
using RDeF.Serialization;

namespace RDeF.Entities
{
    /// <summary>Describes an abstract <see cref="IEntitySource" /> that is serializable.</summary>
    public interface ISerializableEntitySource : IReadableEntitySource
    {
        /// <summary>Writes the content of the entity source into a given <paramref name="rdfWriter" />.</summary>
        /// <param name="streamWriter">Target stream writer.</param>
        /// <param name="rdfWriter">Target RDF writer.</param>
        void Write(StreamWriter streamWriter, IRdfWriter rdfWriter);
    }
}
