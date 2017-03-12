using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using RDeF.Entities;
using RDeF.Vocabularies;

namespace RDeF.Mapping.Converters
{
    /// <summary>Provides conversion for GUID literals.</summary>
    public class GuidConverter : LiteralConverterBase
    {
        private static readonly Iri[] DataTypes = { oguid.Namespace };

        /// <inheritdoc />
        public override IEnumerable<Iri> SupportedDataTypes
        {
            get { return DataTypes; }
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
            return Guid.Parse(statement.Value);
        }

        /// <inheritdoc />
        public override Statement ConvertTo(Iri subject, Iri predicate, object value, Iri graph = null)
        {
            throw new NotSupportedException();
        }
    }
}
