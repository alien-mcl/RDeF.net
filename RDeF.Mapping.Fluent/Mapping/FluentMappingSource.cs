using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using RDeF.Entities;
using RDeF.Mapping.Entities;
using RDeF.Mapping.Explicit;
using RDeF.Mapping.Fluent;
using RDeF.Mapping.Providers;

namespace RDeF.Mapping
{
    /// <summary>Provides mappings defined with attributes.</summary>
    [DebuggerDisplay("Fluent mappings for {_assembly,nq}.")]
    public class FluentMappingSource : IMappingSource
    {
        private readonly Assembly _assembly;
        private readonly IList<ITermMappingProvider> _entityMappingProviders;
        private readonly object _sync = new Object();
        private bool _isInitialized;

        /// <summary>Initializes a new instance of the <see cref="FluentMappingSource"/> class.</summary>
        /// <param name="assembly">The assembly to gather mappings from.</param>
        public FluentMappingSource(Assembly assembly)
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
            var types = from type in assembly.GetExportedTypes()
                        where typeof(IEntity).IsAssignableFrom(type)
                        from entityMapType in assembly.GetExportedTypes()
                        let entityMapTypeInfo = entityMapType.GetTypeInfo()
                        where entityMapTypeInfo.BaseType != null && !entityMapTypeInfo.IsAbstract && entityMapTypeInfo.BaseType.GetTypeInfo().IsGenericType &&
                              entityMapTypeInfo.BaseType.GetGenericTypeDefinition() == typeof(EntityMap<>) &&
                              entityMapTypeInfo.BaseType.GetGenericArguments()[0] == type
                        select new { EntityType = type, EntityMapType = entityMapType };
            foreach (var type in types)
            {
                var entityMap = (DefaultExplicitMappingsBuilder)type.EntityMapType.GetConstructor(Type.EmptyTypes).Invoke(null);
                _entityMappingProviders.AddClasses(type.EntityType, entityMap);
                _entityMappingProviders.AddCollections(type.EntityType, entityMap);
                _entityMappingProviders.AddProperties(type.EntityType, entityMap);
            }
        }
    }
}