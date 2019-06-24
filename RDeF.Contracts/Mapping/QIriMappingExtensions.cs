using System;
using System.Collections.Generic;
using System.Linq;
using RDeF.Entities;

namespace RDeF.Mapping
{
    /// <summary>Provides useful <see cref="QIriMapping" /> extensions.</summary>
    public static class QIriMappingExtensions
    {
        /// <summary>
        /// Resovles a full <see cref="Iri" /> from given either <paramref name="iri" /> or
        /// <paramref name="prefix" />:<paramref name="term" /> and <paramref name="qiriMappings" />s.
        /// </summary>
        /// <param name="qiriMappings">QIri mappings.</param>
        /// <param name="iri">Optional Iri to be resolved.</param>
        /// <param name="prefix">Optional prefix of the QIri to be resolved.</param>
        /// <param name="term">Optional term of the QIri to be resolved.</param>
        /// <returns>Resolved Iri.</returns>
        public static Iri Resolve(this IEnumerable<QIriMapping> qiriMappings, Iri iri, string prefix, string term)
        {
            if (iri != null)
            {
                return iri;
            }

            if ((prefix == null) || (term == null))
            {
                return null;
            }

            var result = (from qIriMapping in qiriMappings
                          where qIriMapping.Prefix == prefix
                          select qIriMapping.Iri).FirstOrDefault();
            if (result == null)
            {
                throw new InvalidOperationException($"Unable to resolve prefix '{prefix}'.");
            }

            return result + term;
        }
    }
}
