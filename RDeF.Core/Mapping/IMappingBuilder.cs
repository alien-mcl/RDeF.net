using System;
using System.Collections.Generic;
using RDeF.Mapping.Providers;

namespace RDeF.Mapping
{
    /// <summary>Describes an abstract facility used for build mappings.</summary>
    public interface IMappingBuilder
    {
        /// <summary>Builds mappings out of given <paramref name="mappingSources" />.</summary>
        /// <param name="mappingSources">Mapping sources.</param>
        /// <param name="openGenericMappingProviders">Map of open generic <see cref="IEntityMappingProvider" />s.</param>
        /// <returns>Map of entity <see cref="Type" /> with it's <see cref="IEntityMapping" /></returns>
        IDictionary<Type, IEntityMapping> BuildMappings(IEnumerable<IMappingSource> mappingSources, IDictionary<Type, ICollection<ITermMappingProvider>> openGenericMappingProviders);

        /// <summary>Builds mappings for a <paramref name="closedGenericType" />.</summary>
        /// <param name="mappings">Mappings to be updated.</param>
        /// <param name="closedGenericType">Type for which to build mappings.</param>
        /// <param name="openGenericEntityMappingProviders">Open generic entity mapping provider.</param>
        void BuildMapping(IDictionary<Type, IEntityMapping> mappings, Type closedGenericType, IEnumerable<ITermMappingProvider> openGenericEntityMappingProviders);
    }
}
