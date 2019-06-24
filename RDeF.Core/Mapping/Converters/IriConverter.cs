using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using RDeF.Entities;

namespace RDeF.Mapping.Converters
{
    /// <summary>Converts statements with object resources into <see cref="Iri" /> values.</summary>
    public class IriConverter : LiteralConverterBase
    {
        /// <inheritdoc />
        public override IEnumerable<Iri> SupportedDataTypes { get; } = Array.Empty<Iri>();

        /// <inheritdoc />
        public override IEnumerable<Type> SupportedTypes { get; } = new[] { typeof(Iri) };

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "There is an assumption in that API that caller will verify whom it is calling.")]
        public override object ConvertFrom(Statement statement)
        {
            return statement.Object;
        }

        /// <inheritdoc />
        public override Statement ConvertTo(Iri subject, Iri predicate, object value, Iri graph = null)
        {
            return new Statement(subject, predicate, (Iri)value, graph);
        }
    }
}
