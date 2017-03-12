using System.Collections.Generic;

namespace RDeF.Mapping
{
    /// <summary>Describes an abstract mapping source.</summary>
    public interface IMappingSource
    {
        /// <summary>Gathers the entity mappings.</summary>
        /// <returns>The entity mappings.</returns>
        IEnumerable<IEntityMapping> GatherEntityMappings();
    }
}
