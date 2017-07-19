using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using RDeF.Entities;
using RDeF.Mapping.Attributes;
using RDeF.Mapping.Providers;

namespace RDeF.Mapping
{
    /// <summary>Provides mappings defined with attributes.</summary>
    [DebuggerDisplay("Attribute mappings for {_assembly,nq}.")]
    public class AttributesMappingSource : IMappingSource
    {
        private readonly Assembly _assembly;
        private readonly List<ITermMappingProvider> _entityMappingProviders;
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
                var classAttributes = type.GetTypeInfo().GetCustomAttributes<ClassAttribute>();
                _entityMappingProviders.AddRange(
                    classAttributes.Select(classAttribute => AttributeEntityMappingProvider.FromAttribute(type, classAttribute)));
                BuildPropertyMappings(type);
            }
        }

        private void BuildPropertyMappings(Type type)
        {
            var propertyAttributes = from property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                     from attribute in property.GetCustomAttributes()
                                     select new { Property = property, Attribute = attribute };
            foreach (var definition in propertyAttributes)
            {
                //// TODO: Add support for dictionaries.
                var collectionAttribute = definition.Attribute as CollectionAttribute;
                if (collectionAttribute != null)
                {
                    _entityMappingProviders.Add(AttributeCollectionMappingProvider.FromAttribute(type, definition.Property, collectionAttribute));
                    continue;
                }

                var propertyAttribute = definition.Attribute as PropertyAttribute;
                if (propertyAttribute != null)
                {
                    _entityMappingProviders.Add(AttributePropertyMappingProvider.FromAttribute(type, definition.Property, propertyAttribute));
                }
            }
        }
    }
}
