using System;
using RDeF.Entities;

namespace RDeF.Mapping
{
    /// <summary>Represents a QIri prefix resolution.</summary>
    public class QIriMapping
    {
        /// <summary>Initializes a new instance of the <see cref="QIriMapping"/> class.</summary>
        /// <param name="prefix">The prefix being mapped.</param>
        /// <param name="iri">Resolution international resource identifier.</param>
        public QIriMapping(string prefix, Iri iri)
        {
            if (prefix == null)
            {
                throw new ArgumentNullException(nameof(prefix));
            }

            if (prefix.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(prefix));
            }

            if (iri == null)
            {
                throw new ArgumentNullException(nameof(iri));
            }

            Prefix = prefix;
            Iri = iri;
        }

        /// <summary>Gets the prefix.</summary>
        public string Prefix { get; }

        /// <summary>Gets the international resource identifier of the <see cref="QIriMapping.Prefix" />.</summary>
        public Iri Iri { get; }
    }
}