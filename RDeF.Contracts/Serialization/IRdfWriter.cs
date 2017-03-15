using System.Collections.Generic;
using System.IO;
using RDeF.Entities;

namespace RDeF.Serialization
{
    /// <summary>Describes an abstract RDF writing facility.</summary>
    public interface IRdfWriter
    {
        /// <summary>Writes RDF <paramref name="graphs"/> into a given <paramref name="streamWriter" />.</summary>
        /// <param name="streamWriter">Stream writer into which RDF data should be written.</param>
        /// <param name="graphs">Named graphs with RDF statements to be written.</param>
        void Write(StreamWriter streamWriter, IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>> graphs);
    }
}
