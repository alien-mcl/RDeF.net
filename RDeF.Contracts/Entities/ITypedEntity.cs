using System.Collections.Generic;

namespace RDeF.Entities
{
    /// <summary>Exposes rdf:type statements as a collection of <see cref="Iri" />.</summary>
    public interface ITypedEntity : IEntity
    {
        /// <summary>Gets the rdf:type statements.</summary>
        /// <remarks>This property provides access to rdf:type statements only in the default graph.</remarks>
        ICollection<Iri> Type { get; }
    }
}
