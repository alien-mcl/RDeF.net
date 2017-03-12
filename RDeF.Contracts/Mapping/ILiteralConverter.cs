using System;
using System.Collections.Generic;
using RDeF.Entities;

namespace RDeF.Mapping
{
    /// <summary>Represents a literal value converter.</summary>
    /// <remarks>Implementations should have a public parameterless default constructor provided.</remarks>
    public interface ILiteralConverter : IConverter
    {
        /// <summary>Gets the collection of supported data types.</summary>
        IEnumerable<Iri> SupportedDataTypes { get; }

        /// <summary>Gets the collection of supported types.</summary>
        IEnumerable<Type> SupportedTypes { get; }
    }
}
