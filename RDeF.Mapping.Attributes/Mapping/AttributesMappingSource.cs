﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using RDeF.Entities;
using RDeF.Mapping.Attributes;
using RDeF.Mapping.Providers;
using RDeF.Mapping.Visitors;

namespace RDeF.Mapping
{
    /// <summary>Provides mappings defined with attributes.</summary>
    [DebuggerDisplay("Attribute mappings for {_assembly,nq}.")]
    public class AttributesMappingSource : IMappingSource
    {
        private readonly Assembly _assembly;
        private readonly IList<ITermMappingProvider> _entityMappingProviders;
        private readonly object _sync = new Object();
        private bool _isInitialized;

        /// <summary>Initializes a new instance of the <see cref="AttributesMappingSource"/> class.</summary>
        /// <param name="assembly">The assembly to gather mappings from.</param>
        public AttributesMappingSource(Assembly assembly)
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
                        select type;
            foreach (var type in types)
            {
                foreach (var classAttribute in type.GetCustomAttributes<ClassAttribute>())
                {
                    _entityMappingProviders.Add(AttributeEntityMappingProvider.FromAttribute(type, classAttribute));
                }

                foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    foreach (var attribute in property.GetCustomAttributes())
                    {
                        //// TODO: Add support for dictionaries.
                        var collectionAttribute = attribute as CollectionAttribute;
                        if (collectionAttribute != null)
                        {
                            _entityMappingProviders.Add(AttributeCollectionMappingProvider.FromAttribute(type, property, collectionAttribute));
                            continue;
                        }

                        var propertyAttribute = attribute as PropertyAttribute;
                        if (propertyAttribute != null)
                        {
                            _entityMappingProviders.Add(AttributePropertyMappingProvider.FromAttribute(type, property, propertyAttribute));
                            continue;
                        }
                    }
                }
            }
        }
    }
}
