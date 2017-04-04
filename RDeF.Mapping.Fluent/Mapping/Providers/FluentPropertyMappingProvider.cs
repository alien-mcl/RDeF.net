using System;
using System.Diagnostics;
using System.Reflection;
using RDeF.Entities;
using RDeF.Mapping.Visitors;

namespace RDeF.Mapping.Providers
{
    /// <summary>Represents a fluent API based property mapping.</summary>
    [DebuggerDisplay("{EntityType.Name,nq}.{Property.Name,nq} ({Iri.ToString()??Prefix+\":\"+Term,nq})")]
    public class FluentPropertyMappingProvider : FluentTermMappingProvider, IPropertyMappingProvider
    {
        internal FluentPropertyMappingProvider(Type entityType, PropertyInfo property, Iri iri, Type valueConverterType = null, Iri graph = null)
            : base(entityType, iri, graph)
        {
            Property = property;
            ValueConverterType = valueConverterType;
        }

        internal FluentPropertyMappingProvider(Type entityType, PropertyInfo property, string prefix, string term, Type valueConverterType = null, Iri graph = null)
            : base(entityType, prefix, term, graph)
        {
            Property = property;
            ValueConverterType = valueConverterType;
        }

        internal FluentPropertyMappingProvider(Type entityType, PropertyInfo property, string prefix, string term, Type valueConverterType = null, string graphPrefix = null, string graphTerm = null)
            : base(entityType, prefix, term, graphPrefix, graphTerm)
        {
            Property = property;
            ValueConverterType = valueConverterType;
        }

        /// <inheritdoc />
        public PropertyInfo Property { get; }

        /// <inheritdoc />
        public Type ValueConverterType { get; set; }

        /// <inheritdoc />
        public override void Accept(IMappingProviderVisitor visitor)
        {
            if (visitor == null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            visitor.Visit(this);
        }
    }
}
