﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using RDeF.Entities;
using RDeF.Vocabularies;

namespace RDeF.Mapping.Converters
{
    /// <summary>Provides conversion for xsd:base64Binary data type.</summary>
    public sealed class Base64BinaryConverter : LiteralConverterBase
    {
        private static readonly Iri[] DataTypes = { xsd.base64Binary };
        private static readonly Type[] Types = { typeof(byte[]) };

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
            return (statement.Value.Length > 0 ? Convert.FromBase64String(statement.Value) : Array.Empty<byte>());
        }

        /// <inheritdoc />
        public override Statement ConvertTo(Iri subject, Iri predicate, object value, Iri graph = null)
        {
            return new Statement(subject, predicate, Convert.ToBase64String((byte[])value), xsd.base64Binary, graph);
        }
    }
}
