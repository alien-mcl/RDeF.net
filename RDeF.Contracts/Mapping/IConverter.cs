using System;
using RDeF.Entities;

namespace RDeF.Mapping
{
    /// <summary>Represents a value converter.</summary>
    /// <remarks>Implementations should have a public parameterless default constructor provided.</remarks>
    public interface IConverter : IEquatable<IConverter>
    {
        /// <summary>Converts from a <paramref name="statement" /> into a strongly typed value.</summary>
        /// <param name="statement">The statement holding either a value or a relation to be converted.</param>
        /// <returns>Strongly typed converted value.</returns>
        object ConvertFrom(Statement statement);

        /// <summary>Converts from a strongly typed <paramref name="value" /> into a <see cref="Statement" />.</summary>
        /// <param name="subject">Subject of the statement.</param>
        /// <param name="predicate">Predicate of the statement.</param>
        /// <param name="value">Value of the statement.</param>
        /// <param name="graph">Optional graph for the statement.</param>
        /// <returns>The <see cref="Statement" />.</returns>
        Statement ConvertTo(Iri subject, Iri predicate, object value, Iri graph = null);
    }
}
