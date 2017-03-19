using System;
using System.Diagnostics;

namespace RDeF.Mapping.Attributes
{
    /// <summary>Represents a collection mapping.</summary>
    [AttributeUsage(AttributeTargets.Property)]
    [DebuggerDisplay("{Iri??Prefix+\":\"+Term,nq}[]")]
    public sealed class CollectionAttribute : PropertyAttribute
    {
        /// <summary>Initializes a new instance of the <see cref="CollectionAttribute"/> class.</summary>
        public CollectionAttribute()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="CollectionAttribute"/> class.</summary>
        /// <param name="prefix">The prefix of the class mapping.</param>
        /// <param name="term">The term of the class mapping.</param>
        /// <param name="graphPrefix">Optional graph prefix.</param>
        /// <param name="graphTerm">Optional graph term.</param>
        public CollectionAttribute(string prefix, string term, string graphPrefix = null, string graphTerm = null) : base(prefix, term, graphPrefix, graphTerm)
        {
        }

        /// <summary>Gets or sets a storage model of the collection being mapped.</summary>
        public CollectionStorageModel StoreAs { get; set; }
    }
}
