using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using RDeF.Entities;
using RDeF.Mapping.Attributes;
using RDeF.Mapping.Visitors;

namespace RDeF.Mapping.Providers
{
    /// <summary>Represents an attribute based property mapping.</summary>
    [DebuggerDisplay("{EntityType.Name,nq}.{Property.Name,nq} ({Iri.ToString()??Prefix+\":\"+Term,nq})")]
    public class AttributePropertyMappingProvider : AttributeTermMappingProvider, IPropertyMappingProvider
    {
        internal AttributePropertyMappingProvider(Type entityType, PropertyInfo property, Iri iri, Type valueConverterType = null, Iri graph = null)
            : base(entityType, iri, graph)
        {
            Property = property;
            ValueConverterType = valueConverterType;
        }

        internal AttributePropertyMappingProvider(Type entityType, PropertyInfo property, string prefix, string term, Type valueConverterType = null, Iri graph = null)
            : base(entityType, prefix, term, graph)
        {
            Property = property;
            ValueConverterType = valueConverterType;
        }

        internal AttributePropertyMappingProvider(Type entityType, PropertyInfo property, string prefix, string term, Type valueConverterType = null, string graphPrefix = null, string graphTerm = null)
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

        internal static AttributePropertyMappingProvider FromAttribute(Type type, PropertyInfo property, PropertyAttribute propertyMapping)
        {
            var types = new List<Type>() { typeof(Type), typeof(PropertyInfo) };
            var parameters = new List<object>() { type, property };
            AddTerm(types, parameters, propertyMapping.MappedIri, propertyMapping.Prefix, propertyMapping.Term);
            types.Add(typeof(Type));
            parameters.Add(propertyMapping.ValueConverterType);
            AddTerm(types, parameters, propertyMapping.GraphIri, propertyMapping.GraphPrefix, propertyMapping.GraphTerm);
            return (AttributePropertyMappingProvider)typeof(AttributePropertyMappingProvider)
                .GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, types.ToArray(), null).Invoke(parameters.ToArray());
        }
    }
}
