using System.Collections.Generic;
using RDeF.Mapping.Providers;

namespace RDeF.Mapping
{
    /// <summary>Describes an abstract mapping source.</summary>
    public interface IMappingSource
    {
        /// <summary>Gathers the mapping providers.</summary>
        /// <returns>Mapping providers.</returns>
        IEnumerable<ITermMappingProvider> GatherEntityMappingProviders();
    }
}
