using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using RDeF.Entities;

namespace RDeF.Mapping.Converters
{
    /// <summary>Provides conversion for untyped literals.</summary>
    public class UntypedLiteralConverter : LiteralConverterBase
    {
        private static readonly Regex GuidRegex = new Regex(@"^[{(]?([0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12})[)}]?$", RegexOptions.IgnoreCase);

        /// <inheritdoc />
        public override IEnumerable<Iri> SupportedDataTypes
        {
            get { return Array.Empty<Iri>(); }
        }

        /// <inheritdoc />
        public override IEnumerable<Type> SupportedTypes
        {
            get { return Array.Empty<Type>(); }
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "There is an assumption in that API that caller will verify whom it is calling.")]
        public override object ConvertFrom(Statement statement)
        {
            if (GuidRegex.IsMatch(statement.Value))
            {
                return Guid.Parse(GuidRegex.Replace(statement.Value, "$1"));
            }

            return statement.Value;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "There is an assumption in that API that caller will verify whom it is calling.")]
        public override Statement ConvertTo(Iri subject, Iri predicate, object value, Iri graph = null)
        {
            return new Statement(subject, predicate, (value is Guid ? $"{{{(Guid)value}}}" : value.ToString()), (Iri)null, graph);
        }
    }
}
