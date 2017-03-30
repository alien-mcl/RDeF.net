using System;
using System.Collections.Generic;
using RDeF.Entities;
using RDeF.Mapping.Providers;
using RDeF.Mapping.Visitors;

namespace RDeF.Mapping
{
    internal sealed class EntityMappingProvider : IEntityMappingProvider
    {
        private readonly IEntityMappingProvider _parentEntityMappingProvider;

        internal EntityMappingProvider(Type entityType, IEntityMappingProvider parentEntityMappingProvider)
        {
            EntityType = entityType;
            _parentEntityMappingProvider = parentEntityMappingProvider;
        }

        /// <inheritdoc />
        public Type EntityType { get; }

        /// <inheritdoc />
        public Iri GetGraph(IEnumerable<QIriMapping> qiriMappings)
        {
            return _parentEntityMappingProvider.GetGraph(qiriMappings);
        }

        /// <inheritdoc />
        public Iri GetTerm(IEnumerable<QIriMapping> qiriMappings)
        {
            return _parentEntityMappingProvider.GetTerm(qiriMappings);
        }

        /// <inheritdoc />
        public void Accept(IMappingProviderVisitor visitor)
        {
            visitor?.Visit(this);
        }
    }
}
