using System;
using System.Diagnostics;
using System.Reflection;
using RDeF.Entities;
using RDeF.Mapping.Reflection;
using RDeF.Mapping.Visitors;

namespace RDeF.Mapping.Providers
{
    /// <summary>Represents a fluent API based property mapping.</summary>
    [DebuggerDisplay("{EntityType.Name,nq}.{Property.Name,nq} ({Iri.ToString()??Prefix+\":\"+Term,nq})")]
    public class FluentPropertyMappingProvider : FluentTermMappingProvider, IPropertyMappingProvider
    {
        private readonly ExplicitlyMappedPropertyInfo _propertyInfo;

        internal FluentPropertyMappingProvider(Type entityType, PropertyInfo property, Iri iri, Type valueConverterType = null, Iri graph = null)
            : base(entityType, iri, graph)
        {
            _propertyInfo = (Property = property) as ExplicitlyMappedPropertyInfo;
            ValueConverterType = valueConverterType;
        }

        /// <inheritdoc />
        public PropertyInfo Property { get; }

        /// <inheritdoc />
        public Type ValueConverterType { get; set; }

        /// <inheritdoc />
        protected override Iri Iri
        {
            get { return _propertyInfo?.Predicate ?? base.Iri; }
        }

        /// <inheritdoc />
        protected override Iri Graph
        {
            get { return _propertyInfo?.Graph ?? base.Graph; }
        }

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
