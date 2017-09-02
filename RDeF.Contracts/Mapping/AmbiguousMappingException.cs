using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace RDeF.Mapping
{
    /// <summary>Represents a situation, when multiple mappings where detected.</summary>
    [Serializable]
    public class AmbiguousMappingException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="AmbiguousMappingException" /> class.</summary>
        public AmbiguousMappingException()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="AmbiguousMappingException" /> class.</summary>
        /// <param name="message">Message of the exception.</param>
        public AmbiguousMappingException(string message) : base(message)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="AmbiguousMappingException" /> class.</summary>
        /// <param name="message">Message of the exception.</param>
        /// <param name="innerException">Inner exception.</param>
        public AmbiguousMappingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="AmbiguousMappingException" /> class.</summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Serialization context.</param>
        [ExcludeFromCodeCoverage]
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "Internally used for deserialization. Implemented only to meet general requirements. Contains no special logic.")]
        protected AmbiguousMappingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
