using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace RDeF.Mapping.Attributes
{
    /// <summary>Represents a property mapping.</summary>
    [SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", Justification = "Hierarchy is rquired and constistant with corresponding types.")]
    [AttributeUsage(AttributeTargets.Property)]
    [DebuggerDisplay("{Iri??Prefix+\":\"+Term,nq}")]
    public class PropertyAttribute : TermAttribute
    {
        private Type _valueConverterType;

        /// <summary>Initializes a new instance of the <see cref="PropertyAttribute"/> class.</summary>
        public PropertyAttribute()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="PropertyAttribute"/> class.</summary>
        /// <param name="prefix">The prefix of the class mapping.</param>
        /// <param name="term">The term of the class mapping.</param>
        /// <param name="graphPrefix">Optional graph prefix.</param>
        /// <param name="graphTerm">Optional graph term.</param>
        public PropertyAttribute(string prefix, string term, string graphPrefix = null, string graphTerm = null) : base(prefix, term, graphPrefix, graphTerm)
        {
        }

        /// <summary>Gets or sets the type of the converter.</summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Using setter's value would be meaningless.")]
        public Type ValueConverterType
        {
            get
            {
                return _valueConverterType;
            }

            set
            {
                if ((value != null) && (!typeof(IConverter).IsAssignableFrom(value)))
                {
                    throw new ArgumentOutOfRangeException("valueConverterType");
                }

                _valueConverterType = value;
            }
        }
    }
}
