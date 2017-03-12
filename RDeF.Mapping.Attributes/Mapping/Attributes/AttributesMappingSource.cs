using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using RDeF.ComponentModel;
using RDeF.Entities;

namespace RDeF.Mapping.Attributes
{
    /// <summary>Provides mappings defined with attributes.</summary>
    [DebuggerDisplay("Attribute mappings for {_assembly,nq}.")]
    public class AttributesMappingSource : IMappingSource
    {
        private readonly IActivator _activator;
        private readonly Assembly _assembly;
        private readonly IList<IEntityMapping> _entityMappings;
        private readonly IEnumerable<QIriMapping> _qiriMappings;
        private readonly object _sync = new Object();
        private bool _isInitialized;

        /// <summary>Initializes a new instance of the <see cref="AttributesMappingSource"/> class.</summary>
        /// <param name="activator">Facility creating object instances.</param>
        /// <param name="assembly">The assembly to gather mappings from.</param>
        /// <param name="qiriMappings">Optional QIri mappings.</param>
        public AttributesMappingSource(IActivator activator, Assembly assembly, IEnumerable<QIriMapping> qiriMappings = null)
        {
            if (activator == null)
            {
                throw new ArgumentNullException(nameof(activator));
            }

            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            _entityMappings = new List<IEntityMapping>();
            _activator = activator;
            _assembly = assembly;
            _qiriMappings = qiriMappings ?? Array.Empty<QIriMapping>();
        }

        /// <inheritdoc />
        public IEnumerable<IEntityMapping> GatherEntityMappings()
        {
            if (!_isInitialized)
            {
                lock (_sync)
                {
                    BuildMappings(_assembly, _qiriMappings);
                    _isInitialized = true;
                }
            }

            return _entityMappings;
        }

        private void BuildMappings(Assembly assembly, IEnumerable<QIriMapping> qiriMappings)
        {
            var types = from type in assembly.GetExportedTypes()
                        where typeof(IEntity).IsAssignableFrom(type)
                        select type;
            foreach (var type in types)
            {
                var entityMapping = new AttributeEntityMapping(type);
                foreach (var classAttribute in type.GetCustomAttributes<ClassAttribute>())
                {
                    entityMapping.Classes.Add(classAttribute.Resolve(qiriMappings));
                }

                foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    //// TODO: Add support for collections.
                    foreach (var propertyAttribute in property.GetCustomAttributes<PropertyAttribute>(true))
                    {
                        IConverter converter = null;
                        if (propertyAttribute.ValueConverterType != null)
                        {
                            converter = (IConverter)_activator.CreateInstance(propertyAttribute.ValueConverterType);
                        }

                        entityMapping.Properties.Add(
                            new AttributePropertyMapping(entityMapping, property.Name, propertyAttribute.Resolve(qiriMappings), propertyAttribute.Graph, converter));
                    }
                }

                if ((entityMapping.Classes.Count > 0) || (entityMapping.Properties.Count > 0))
                {
                    _entityMappings.Add(entityMapping);
                }
            }
        }
    }
}
