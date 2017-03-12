using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using RDeF.Entities;

namespace RDeF.Mapping.Attributes
{
    /// <summary>Represents a term mapping.</summary>
    public abstract class TermAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="TermAttribute"/> class.</summary>
        /// <param name="prefix">The prefix of the class mapping.</param>
        /// <param name="term">The term of the class mapping.</param>
        /// <param name="graph">Optional graph.</param>
        internal TermAttribute(string prefix, string term, string graph = null)
        {
            if (prefix == null)
            {
                throw new ArgumentNullException(nameof(prefix));
            }

            if (prefix.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(prefix));
            }

            if (term == null)
            {
                throw new ArgumentNullException(nameof(term));
            }

            if (term.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(term));
            }

            Prefix = prefix;
            Term = term;
            Graph = (graph != null ? new Iri(graph) : null);
        }

        /// <summary>Initializes a new instance of the <see cref="TermAttribute"/> class.</summary>
        /// <param name="iri">The iri.</param>
        /// <param name="graph">Optional graph.</param>
        internal TermAttribute(string iri, string graph = null)
        {
            if (iri == null)
            {
                throw new ArgumentNullException(nameof(iri));
            }

            if (iri.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(iri));
            }

            Iri = new Iri(iri);
            Graph = (graph != null ? new Iri(graph) : null);
        }

        internal Iri Graph { get; }

        internal string Prefix { get; }

        internal string Term { get; }

        internal Iri Iri { get; }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return Iri?.ToString() ?? $"{Prefix}:{Term}";
        }

        internal Iri Resolve(IEnumerable<QIriMapping> qiriMappings)
        {
            if (Iri != null)
            {
                return Iri;
            }

            var result = (from qIriMapping in qiriMappings
                          where qIriMapping.Prefix == Prefix
                          select qIriMapping.Iri).FirstOrDefault();
            if (result == null)
            {
                throw new InvalidOperationException($"Unable to resolve prefix '{Prefix}'.");
            }

            return result + Term;
        }
    }
}
