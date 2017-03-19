using System.Collections.Generic;
using System.Reflection;

namespace RDeF.Mapping
{
    /// <summary>Provides <see cref="IMappingSource" />s.</summary>
    public class AttributesMappingSourceProvider : IMappingSourceProvider
    {
        /// <inheritdoc/>
        public IEnumerable<IMappingSource> GetMappingSourcesFor(Assembly assembly)
        {
            return new[] { new AttributesMappingSource(assembly) };
        }
    }
}
