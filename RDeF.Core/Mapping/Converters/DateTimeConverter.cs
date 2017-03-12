using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Xml;
using RDeF.Entities;
using RDeF.Vocabularies;

namespace RDeF.Mapping.Converters
{
    /// <summary>Provides conversion for xsd:dateTime, xsd:date and data types.</summary>
    public sealed class DateTimeConverter : LiteralConverterBase
    {
        private static readonly Regex TimeRegex = new Regex("^[0-9]{2}:[0-9]{2}:[0-9]{2}((Z)|([-+][0-9]{2}:[0-9]{2}))?$");
        private static readonly Iri[] DataTypes = { xsd.dateTime, xsd.date, xsd.time };
        private static readonly Type[] Types = { typeof(DateTime) };

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
            var dateTime = XmlConvert.ToDateTime(statement.Value, XmlDateTimeSerializationMode.RoundtripKind);
            if (dateTime.Kind == DateTimeKind.Local)
            {
                dateTime = dateTime.ToUniversalTime();
            }

            if (TimeRegex.IsMatch(statement.Value))
            {
                dateTime = dateTime
                    .AddYears(-dateTime.Year + default(DateTime).Year)
                    .AddMonths(-dateTime.Month + default(DateTime).Month)
                    .AddDays(-dateTime.Day + default(DateTime).Day);
            }

            return dateTime;
        }

        /// <inheritdoc />
        public override Statement ConvertTo(Iri subject, Iri predicate, object value, Iri graph = null)
        {
            DateTime dateTime = (DateTime)value;
            string literalValue;
            Iri dataType;
            if ((dateTime.Year == default(DateTime).Year) && (dateTime.Month == default(DateTime).Month) && (dateTime.Day == default(DateTime).Day))
            {
                literalValue = XmlConvert.ToString((DateTime)value, XmlDateTimeSerializationMode.RoundtripKind).Substring(11);
                dataType = xsd.time;
            }
            else if ((dateTime.Hour == default(DateTime).Hour) && (dateTime.Minute == default(DateTime).Minute) &&
                     (dateTime.Second == default(DateTime).Second) && (dateTime.Millisecond == default(DateTime).Millisecond))
            {
                literalValue = XmlConvert.ToString((DateTime)value, XmlDateTimeSerializationMode.Utc).Substring(0, 10);
                dataType = xsd.date;
            }
            else
            {
                literalValue = XmlConvert.ToString((DateTime)value, XmlDateTimeSerializationMode.RoundtripKind);
                dataType = xsd.dateTime;
            }

            return new Statement(subject, predicate, literalValue, dataType, graph);
        }
    }
}
