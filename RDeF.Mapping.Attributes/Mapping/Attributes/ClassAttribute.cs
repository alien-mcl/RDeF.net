using System;
using System.Diagnostics;

namespace RDeF.Mapping.Attributes
{
    /// <summary>Represents a class mapping.</summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = true)]
    [DebuggerDisplay("a {ToString()}")]
    public sealed class ClassAttribute : TermAttribute
    {
        /// <summary>Initializes a new instance of the <see cref="ClassAttribute"/> class.</summary>
        /// <param name="prefix">The prefix of the class mapping.</param>
        /// <param name="term">The term of the class mapping.</param>
        /// <param name="graph">Optional graph.</param>
        public ClassAttribute(string prefix, string term, string graph = null) : base(prefix, term, graph)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ClassAttribute"/> class.</summary>
        /// <param name="iri">The iri.</param>
        /// <param name="graph">Optional graph.</param>
        public ClassAttribute(string iri, string graph = null) : base(iri, graph)
        {
        }
    }
}
