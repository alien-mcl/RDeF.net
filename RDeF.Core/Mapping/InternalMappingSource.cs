using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RDeF.Entities;
using RDeF.Mapping.Converters;
using RDeF.Mapping.Providers;
using RDeF.Vocabularies;

namespace RDeF.Mapping
{
    /// <summary>Provides internally defined <see cref="IStatementMapping" />s.</summary>
    public class InternalMappingSource : IMappingSource
    {
        private readonly Assembly _assembly;
        private readonly List<ITermMappingProvider> _entityMappingProviders;
        private readonly object _sync = new Object();
        private bool _isInitialized;

        internal InternalMappingSource(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            _entityMappingProviders = new List<ITermMappingProvider>();
            _assembly = assembly;
        }

        /// <inheritdoc />
        public IEnumerable<ITermMappingProvider> GatherEntityMappingProviders()
        {
            if (!_isInitialized)
            {
                lock (_sync)
                {
                    BuildMappings(_assembly);
                    _isInitialized = true;
                }
            }

            return _entityMappingProviders;
        }

        private void BuildMappings(Assembly assembly)
        {
            BuildPropertyMappings(assembly.GetExportedTypes().First(type => type.Name == nameof(ITypedEntity)));
        }

        private void BuildPropertyMappings(Type type)
        {
            _entityMappingProviders.Add(
                new InternalCollectionMappingProvider(
                    type,
                    type.GetProperty(nameof(ITypedEntity.Type)),
                    rdf.type,
                    CollectionStorageModel.Simple,
                    typeof(IriConverter)));
        }
    }
}
