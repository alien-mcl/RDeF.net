using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using RDeF.Entities;
using RDeF.Mapping.Visitors;

namespace RDeF.Mapping.Providers
{
    /// <summary>Represents an attribute based collection mapping.</summary>
    [DebuggerDisplay("{EntityType.Name,nq}.{Property.Name,nq}[] ({Iri.ToString()??Prefix+\":\"+Term,nq})")]
    public sealed class InternalCollectionMappingProvider : ICollectionMappingProvider
    {
        private readonly Iri _iri;

        internal InternalCollectionMappingProvider(
            Type entityType,
            PropertyInfo property,
            Iri iri,
            CollectionStorageModel storeAs,
            Type valueConverterType)
        {
            EntityType = entityType;
            Property = property;
            ValueConverterType = valueConverterType;
            StoreAs = storeAs;
            _iri = iri;
        }

        /// <inheritdoc />
        public Type EntityType { get; }

        /// <summary>Gets the property being mapped.</summary>
        public PropertyInfo Property { get; }

        /// <summary>Gets or sets the value converter type the mapping.</summary>
        public Type ValueConverterType { get; set; }

        /// <inheritdoc />
        public CollectionStorageModel StoreAs { get; set; }

        /// <inheritdoc />
        public Iri GetGraph(IEnumerable<QIriMapping> qiriMappings)
        {
            return null;
        }

        /// <inheritdoc />
        public Iri GetTerm(IEnumerable<QIriMapping> qiriMappings)
        {
            return qiriMappings.Resolve(_iri, null, null);
        }

        /// <inheritdoc />
        public void Accept(IMappingProviderVisitor visitor)
        {
            if (visitor == null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            visitor.Visit(this);
        }
    }
}
