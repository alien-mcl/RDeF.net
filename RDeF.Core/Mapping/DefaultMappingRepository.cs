using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using RDeF.Entities;

namespace RDeF.Mapping
{
    /// <summary>Provides a default implementation of the <see cref="IMappingsRepository" /> which gathers mapping from various sources.</summary>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "The type is not considered a collection.")]
    public sealed class DefaultMappingRepository : IMappingsRepository
    {
        /// <summary>Initializes a new instance of the <see cref="DefaultMappingRepository"/> class.</summary>
        /// <param name="mappingSources">Collection of mapping sources.</param>
        public DefaultMappingRepository(IEnumerable<IMappingSource> mappingSources)
        {
            Mappings = new ConcurrentDictionary<Type, MergingEntityMapping>();
            BuildMappings(mappingSources);
        }

        internal ConcurrentDictionary<Type, MergingEntityMapping> Mappings { get; }

        /// <inheritdoc />
        public IEntityMapping FindEntityMappingFor(Iri @class)
        {
            if (@class == null)
            {
                throw new ArgumentNullException(nameof(@class));
            }

            return (from entityMapping in Mappings.Values
                    from mappedClass in entityMapping.Classes
                    where mappedClass == @class
                    select entityMapping).FirstOrDefault();
        }

        /// <inheritdoc />
        public IEntityMapping FindEntityMappingFor(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return (from entityMapping in Mappings.Values
                    where entityMapping.Type == type
                    select entityMapping).FirstOrDefault();
        }

        /// <inheritdoc />
        public IPropertyMapping FindPropertyMappingFor(Iri predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return (from entityMapping in Mappings.Values
                    from propertyMapping in entityMapping.Properties
                    where propertyMapping.Predicate == predicate
                    select propertyMapping).FirstOrDefault();
        }

        /// <inheritdoc />
        public IPropertyMapping FindPropertyMappingFor(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            return (from entityMapping in Mappings.Values
                    where entityMapping.Type == property.DeclaringType
                    from propertyMapping in entityMapping.Properties
                    where propertyMapping.Name == property.Name
                    select propertyMapping).FirstOrDefault();
        }

        /// <inheritdoc />
        public IEnumerator<IEntityMapping> GetEnumerator()
        {
            return Mappings.Values.GetEnumerator();
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void BuildMappings(IEnumerable<IMappingSource> mappingSources)
        {
            lock (Mappings)
            {
                foreach (var mappingSource in mappingSources)
                {
                    foreach (var entityMapping in mappingSource.GatherEntityMappings())
                    {
                        MergingEntityMapping existingEntityMapping;
                        if (!Mappings.TryGetValue(entityMapping.Type, out existingEntityMapping))
                        {
                            Mappings[entityMapping.Type] = existingEntityMapping = new MergingEntityMapping(entityMapping.Type);
                        }

                        foreach (var @class in entityMapping.Classes)
                        {
                            existingEntityMapping.Classes.Add(@class);
                        }

                        foreach (var propertyMapping in entityMapping.Properties)
                        {
                            existingEntityMapping.Properties.Add(propertyMapping);
                        }
                    }
                }
            }
        }
    }
}
