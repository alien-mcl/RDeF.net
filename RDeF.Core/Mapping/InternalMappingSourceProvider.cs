using System;
using System.Collections.Generic;
using System.Reflection;
using RDeF.Entities;

namespace RDeF.Mapping
{
    /// <summary>Provides internally defined <see cref="IMappingSource" />.</summary>
    public class InternalMappingSourceProvider : IMappingSourceProvider
    {
        /// <inheritdoc />
        public IEnumerable<IMappingSource> GetMappingSourcesFor(Assembly assembly)
        {
            return assembly == typeof(ITypedEntity).Assembly
                ? new[] { new InternalMappingSource(assembly) }
                : Array.Empty<IMappingSource>();
        }
    }
}
