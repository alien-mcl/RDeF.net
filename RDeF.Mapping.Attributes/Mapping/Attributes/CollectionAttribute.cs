using System;
using System.Diagnostics;

namespace RDeF.Mapping.Attributes
{
    /// <summary>Represents a collection mapping.</summary>
    [AttributeUsage(AttributeTargets.Property)]
    [DebuggerDisplay("{ToString()}")]
    public sealed class CollectionAttribute : TermAttribute
    {
        /// <summary>Initializes a new instance of the <see cref="CollectionAttribute"/> class.</summary>
        /// <param name="prefix">The prefix of the class mapping.</param>
        /// <param name="term">The term of the class mapping.</param>
        /// <param name="valueConverterType">Type of the converter to be used.</param>
        /// <param name="graph">Optional graph.</param>
        public CollectionAttribute(string prefix, string term, Type valueConverterType = null, string graph = null) : base(prefix, term, graph)
        {
            if ((valueConverterType != null) && (!typeof(IConverter).IsAssignableFrom(valueConverterType)))
            {
                throw new ArgumentOutOfRangeException(nameof(valueConverterType));
            }

            ValueConverterType = valueConverterType;
        }

        /// <summary>Initializes a new instance of the <see cref="CollectionAttribute"/> class.</summary>
        /// <param name="iri">The iri.</param>
        /// <param name="valueConverterType">Type of the converter to be used.</param>
        /// <param name="graph">Optional graph.</param>
        public CollectionAttribute(string iri, Type valueConverterType = null, string graph = null) : base(iri, graph)
        {
            if ((valueConverterType != null) && (!typeof(IConverter).IsAssignableFrom(valueConverterType)))
            {
                throw new ArgumentOutOfRangeException(nameof(valueConverterType));
            }

            ValueConverterType = valueConverterType;
        }

        /// <summary>Gets the type of the converter.</summary>
        internal Type ValueConverterType { get; }
    }
}
