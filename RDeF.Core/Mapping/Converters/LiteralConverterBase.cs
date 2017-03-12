using System;
using System.Collections.Generic;
using RDeF.Entities;

namespace RDeF.Mapping.Converters
{
    /// <summary>Provides a base funcationality for literal converters.</summary>
    public abstract class LiteralConverterBase : ILiteralConverter
    {
        /// <inheritdoc />
        public abstract IEnumerable<Iri> SupportedDataTypes { get; }

        /// <inheritdoc />
        public abstract IEnumerable<Type> SupportedTypes { get; }

        /// <inheritdoc />
        public virtual bool Equals(IConverter other)
        {
            return other?.GetType() == GetType();
        }

        /// <inheritdoc />
        public abstract object ConvertFrom(Statement statement);

        /// <inheritdoc />
        public abstract Statement ConvertTo(Iri subject, Iri predicate, object value, Iri graph = null);
    }
}
