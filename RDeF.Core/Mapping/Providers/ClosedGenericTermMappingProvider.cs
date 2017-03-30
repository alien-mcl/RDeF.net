using System;
using System.Collections.Generic;
using RDeF.Entities;
using RDeF.Mapping.Visitors;

namespace RDeF.Mapping.Providers
{
    internal abstract class ClosedGenericTermMappingProvider : ITermMappingProvider
    {
        private readonly ITermMappingProvider _openGenericTermMappingProvider;

        internal ClosedGenericTermMappingProvider(Type closedGenericType, ITermMappingProvider openGenericTermMappingProvider)
        {
            EntityType = closedGenericType;
            _openGenericTermMappingProvider = openGenericTermMappingProvider;
        }

        /// <inheritdoc />
        public Type EntityType { get; }

        /// <inheritdoc />
        public Iri GetGraph(IEnumerable<QIriMapping> qiriMappings)
        {
            return _openGenericTermMappingProvider.GetGraph(qiriMappings);
        }

        /// <inheritdoc />
        public Iri GetTerm(IEnumerable<QIriMapping> qiriMappings)
        {
            return _openGenericTermMappingProvider.GetTerm(qiriMappings);
        }

        /// <inheritdoc />
        public abstract void Accept(IMappingProviderVisitor visitor);
    }
}
