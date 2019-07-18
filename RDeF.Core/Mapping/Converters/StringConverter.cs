using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using RDeF.Entities;
using RDeF.Vocabularies;

namespace RDeF.Mapping.Converters
{
    /// <summary>Provides conversion for xsd:string data type.</summary>
    public class StringConverter : LiteralConverterBase
    {
        private static readonly Iri[] DataTypes = { xsd.@string, rdf.langString };
        private static readonly Type[] Types = { typeof(string) };

        /// <inheritdoc />
        public override IEnumerable<Iri> SupportedDataTypes
        {
            get { return DataTypes; }
        }

        /// <inheritdoc />
        public override IEnumerable<Type> SupportedTypes
        {
            get { return Types; }
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "There is an assumption in that API that caller will verify whom it is calling.")]
        public override object ConvertFrom(Statement statement)
        {
            return statement.Value;
        }

        /// <inheritdoc />
        public override Statement ConvertTo(Iri subject, Iri predicate, object value, Iri graph = null)
        {
            return new Statement(subject, predicate, (string)value, xsd.@string, graph);
        }
    }
}
