using System;
using System.Collections.Generic;
using RDeF.Entities;
using RDeF.Mapping.Visitors;

namespace RDeF.Mapping.Providers
{
    /// <summary>Represents an abstract term mapping provider.</summary>
    public interface ITermMappingProvider
    {
        /// <summary>Gets the type of the entity being mapped.</summary>
        Type EntityType { get; }

        /// <summary>Gets the optional graph requirement by the mapping.</summary>
        /// <param name="qiriMappings">QIri mappings.</param>
        /// <returns>Graph required by the mapping.</returns>
        Iri GetGraph(IEnumerable<QIriMapping> qiriMappings);

        /// <summary>Gets the term being mapped.</summary>
        /// <param name="qiriMappings">QIri mappings.</param>
        /// <returns>Term being mapping.</returns>
        Iri GetTerm(IEnumerable<QIriMapping> qiriMappings);

        /// <summary>Visits a given <paramref name="visitor" />.</summary>
        /// <param name="visitor">Mapping visitor.</param>
        void Accept(IMappingProviderVisitor visitor);
    }
}