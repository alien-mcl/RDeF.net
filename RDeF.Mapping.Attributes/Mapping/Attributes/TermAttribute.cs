using System;
using System.Diagnostics.CodeAnalysis;
using RDeF.Entities;

namespace RDeF.Mapping.Attributes
{
    /// <summary>Represents a term mapping.</summary>
    public abstract class TermAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="TermAttribute"/> class.</summary>
        protected TermAttribute()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="TermAttribute"/> class.</summary>
        /// <param name="prefix">The prefix of the class mapping.</param>
        /// <param name="term">The term of the class mapping.</param>
        /// <param name="graphPrefix">Optional graph prefix.</param>
        /// <param name="graphTerm">Optional graph term.</param>
        protected TermAttribute(string prefix, string term, string graphPrefix = null, string graphTerm = null)
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
            GraphPrefix = graphPrefix;
            GraphTerm = graphTerm;
        }

        /// <summary>Gets or sets a mapped term's <see cref="Iri" />.</summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Using setter's value would be meaningless.")]
        public string Iri
        {
            get
            {
                return MappedIri?.ToString();
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("iri");
                }

                if (value.Length == 0)
                {
                    throw new ArgumentOutOfRangeException("iri");
                }

                MappedIri = new Iri(value);
            }
        }

        /// <summary>Gets or sets a required optional graph's <see cref="Iri" />.</summary>
        public string Graph
        {
            get { return GraphIri?.ToString(); }
            set { GraphIri = (value != null ? new Iri(value) : null); }
        }

        internal string GraphPrefix { get; }

        internal string GraphTerm { get; }

        internal string Prefix { get; }

        internal string Term { get; }

        internal Iri MappedIri { get; private set; }

        internal Iri GraphIri { get; private set; }
    }
}
