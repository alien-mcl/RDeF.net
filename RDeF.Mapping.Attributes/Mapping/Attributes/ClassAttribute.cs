using System;
using System.Diagnostics;

namespace RDeF.Mapping.Attributes
{
    /// <summary>Represents a class mapping.</summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = true)]
    [DebuggerDisplay("a {Iri??Prefix+\":\"+Term,nq}")]
    public sealed class ClassAttribute : TermAttribute
    {
        /// <summary>Initializes a new instance of the <see cref="ClassAttribute"/> class.</summary>
        public ClassAttribute()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ClassAttribute"/> class.</summary>
        /// <param name="prefix">The prefix of the class mapping.</param>
        /// <param name="term">The term of the class mapping.</param>
        /// <param name="graphPrefix">Optional graph prefix.</param>
        /// <param name="graphTerm">Optional graph term.</param>
        public ClassAttribute(string prefix, string term, string graphPrefix = null, string graphTerm = null) : base(prefix, term, graphPrefix, graphTerm)
        {
        }
    }
}
