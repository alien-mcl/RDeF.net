using System;
using System.Collections.Generic;
using System.Reflection;

namespace RDeF.Mapping
{
    /// <summary>Provides fluent-like API <see cref="IMappingSource" />s.</summary>
    public class FluentMappingSourceProvider : IMappingSourceProvider
    {
        private readonly IEnumerable<QIriMapping> _qiriMappings;

        /// <summary>Initializes a new instance of the <see cref="FluentMappingSourceProvider" /> class.</summary>
        /// <param name="qiriMappings">qIri mappings.</param>
        public FluentMappingSourceProvider(IEnumerable<QIriMapping> qiriMappings)
        {
            if (qiriMappings == null)
            {
                throw new ArgumentNullException(nameof(qiriMappings));
            }

            _qiriMappings = qiriMappings;
        }

        /// <inheritdoc/>
        public IEnumerable<IMappingSource> GetMappingSourcesFor(Assembly assembly)
        {
            return new[] { new FluentMappingSource(assembly, _qiriMappings) };
        }
    }
}
