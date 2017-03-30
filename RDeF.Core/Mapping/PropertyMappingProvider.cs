using System;
using System.Collections.Generic;
using System.Reflection;
using RDeF.Entities;
using RDeF.Mapping.Providers;
using RDeF.Mapping.Visitors;

namespace RDeF.Mapping
{
    internal class PropertyMappingProvider : IPropertyMappingProvider
    {
        private readonly IPropertyMappingProvider _parentPropertyMappingProvider;

        internal PropertyMappingProvider(Type entityType, IPropertyMappingProvider parentPropertyMappingProvider)
        {
            EntityType = entityType;
            _parentPropertyMappingProvider = parentPropertyMappingProvider;
        }

        /// <inheritdoc />
        public Type EntityType { get; }

        /// <inheritdoc />
        public PropertyInfo Property { get { return _parentPropertyMappingProvider.Property; } }

        /// <inheritdoc />
        public Type ValueConverterType
        {
            get { return _parentPropertyMappingProvider.ValueConverterType; }
            set { _parentPropertyMappingProvider.ValueConverterType = value; }
        }

        /// <inheritdoc />
        public Iri GetGraph(IEnumerable<QIriMapping> qiriMappings)
        {
            return _parentPropertyMappingProvider.GetGraph(qiriMappings);
        }

        /// <inheritdoc />
        public Iri GetTerm(IEnumerable<QIriMapping> qiriMappings)
        {
            return _parentPropertyMappingProvider.GetTerm(qiriMappings);
        }

        /// <inheritdoc />
        public void Accept(IMappingProviderVisitor visitor)
        {
            visitor?.Visit(this);
        }
    }
}
