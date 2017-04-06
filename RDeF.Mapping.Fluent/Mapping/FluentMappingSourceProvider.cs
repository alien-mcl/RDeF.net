using System.Collections.Generic;
using System.Reflection;

namespace RDeF.Mapping
{
    /// <summary>Provides fluent-like API <see cref="IMappingSource" />s.</summary>
    public class FluentMappingSourceProvider : IMappingSourceProvider
    {
        /// <inheritdoc/>
        public IEnumerable<IMappingSource> GetMappingSourcesFor(Assembly assembly)
        {
            return new[] { new FluentMappingSource(assembly) };
        }
    }
}
