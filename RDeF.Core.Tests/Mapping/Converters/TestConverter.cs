using System;
using System.Collections.Generic;
using RDeF.Entities;

namespace RDeF.Mapping.Converters
{
    /// <summary>Test converter.</summary>
    public class TestConverter : ILiteralConverter
    {
        /// <inheritdoc />
        public IEnumerable<Iri> SupportedDataTypes
        {
            get { return new[] { new Iri("test") }; }
        }

        /// <inheritdoc />
        public IEnumerable<Type> SupportedTypes
        {
            get { return new[] { typeof(object) }; }
        }

        /// <inheritdoc />
        public bool Equals(IConverter other)
        {
            return other is TestConverter;
        }

        /// <inheritdoc />
        public object ConvertFrom(Statement statement)
        {
            return statement.Value;
        }

        /// <inheritdoc />
        public Statement ConvertTo(Iri subject, Iri predicate, object value, Iri graph = null)
        {
            return new Statement(subject, predicate, value.ToString());
        }
    }
}
