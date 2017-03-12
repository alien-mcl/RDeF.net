using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Xml;
using RDeF.Entities;
using RDeF.Vocabularies;

namespace RDeF.Mapping.Converters
{
    /// <summary>Provides conversion for integral data types.</summary>
    public sealed class IntegerConverter : LiteralConverterBase
    {
        private static readonly Dictionary<Type, ISet<Iri>> IntegralTypes = new Dictionary<Type, ISet<Iri>>()
        {
            { typeof(sbyte), new HashSet<Iri>() { xsd.@byte } },
            { typeof(byte), new HashSet<Iri>() { xsd.unsignedByte } },
            { typeof(short), new HashSet<Iri>() { xsd.@short } },
            { typeof(ushort), new HashSet<Iri>() { xsd.unsignedShort } },
            { typeof(int), new HashSet<Iri>() { xsd.@int } },
            { typeof(uint), new HashSet<Iri>() { xsd.unsignedInt } },
            { typeof(long), new HashSet<Iri>() { xsd.@long, xsd.integer, xsd.nonPositiveInteger, xsd.unsignedInteger } },
            { typeof(ulong), new HashSet<Iri>() { xsd.unsignedLong, xsd.nonNegativeInteger, xsd.positiveInteger } },
        };

        /// <inheritdoc />
        public override IEnumerable<Iri> SupportedDataTypes
        {
            get { return IntegralTypes.Values.SelectMany(dataType => dataType); }
        }

        /// <inheritdoc />
        public override IEnumerable<Type> SupportedTypes
        {
            get { return IntegralTypes.Keys; }
        }

        /// <inheritdoc />
        public override object ConvertFrom(Statement statement)
        {
            var returnType = typeof(Int64);
            if (statement.DataType != null)
            {
                returnType = IntegralTypes.Single(pair => pair.Value.Contains(statement.DataType)).Key;
            }

            return Convert.ChangeType(statement.Value, returnType);
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "There is an assumption in that API that caller will verify whom it is calling.")]
        public override Statement ConvertTo(Iri subject, Iri predicate, object value, Iri graph = null)
        {
            return new Statement(subject, predicate, XmlConvert.ToString((dynamic)value), IntegralTypes[value.GetType()].First(), graph);
        }
    }
}
