using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using RDeF.Entities;
using RDeF.Vocabularies;

namespace RDeF.Mapping.Converters
{
    /// <summary>Provides conversion for xsd:float and xsd:double data types.</summary>
    public sealed class FloatingPointLiteralConverter : LiteralConverterBase
    {
        private static readonly Iri[] DataTypes = { xsd.@float, xsd.@double };
        private static readonly Type[] Types = { typeof(float), typeof(double) };

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
            if ((statement.Value == "+INF") || (statement.Value == "INF"))
            {
                return (statement.DataType == xsd.@float ? (object)float.PositiveInfinity : double.PositiveInfinity);
            }

            if (statement.Value == "-INF")
            {
                return (statement.DataType == xsd.@float ? (object)float.NegativeInfinity : double.NegativeInfinity);
            }

            return (statement.DataType == xsd.@float ? (object)XmlConvert.ToSingle(statement.Value) : XmlConvert.ToDouble(statement.Value));
        }

        /// <inheritdoc />
        public override Statement ConvertTo(Iri subject, Iri predicate, object value, Iri graph = null)
        {
            if (value is float)
            {
                return new Statement(subject, predicate, XmlConvert.ToString((float)value), xsd.@float, graph);
            }

            return new Statement(subject, predicate, XmlConvert.ToString((double)value), xsd.@double, graph);
        }
    }
}
