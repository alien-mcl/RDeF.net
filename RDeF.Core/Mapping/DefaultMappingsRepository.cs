using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using RDeF.Entities;
using RDeF.Mapping.Providers;

namespace RDeF.Mapping
{
    /// <summary>Provides a default implementation of the <see cref="IMappingsRepository" /> which gathers mapping from various sources.</summary>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "The type is not considered a collection.")]
    public class DefaultMappingsRepository : IMappingsRepository
    {
        private readonly IMappingBuilder _mappingBuilder;
        private readonly IDictionary<Type, IEntityMapping> _mappings;
        private readonly IDictionary<Type, ICollection<ITermMappingProvider>> _openGenericProviders;

        /// <summary>Initializes a new instance of the <see cref="DefaultMappingsRepository"/> class.</summary>
        /// <param name="mappingSources">Collection of mapping sources.</param>
        /// <param name="mappingsBuilder">Mapping builder.</param>
        public DefaultMappingsRepository(IEnumerable<IMappingSource> mappingSources, IMappingBuilder mappingsBuilder)
        {
            if (mappingSources == null)
            {
                throw new ArgumentNullException(nameof(mappingSources));
            }

            if (mappingsBuilder == null)
            {
                throw new ArgumentNullException(nameof(mappingsBuilder));
            }

            _openGenericProviders = new ConcurrentDictionary<Type, ICollection<ITermMappingProvider>>();
            _mappings = (_mappingBuilder = mappingsBuilder).BuildMappings(mappingSources, _openGenericProviders);
        }

        /// <inheritdoc />
        public virtual IEntityMapping FindEntityMappingFor(IEntity entity, Iri @class, Iri graph = null)
        {
            if (@class == null)
            {
                throw new ArgumentNullException(nameof(@class));
            }

            return (from entityMapping in _mappings.Values
                    from mappedClass in entityMapping.Classes
                    where mappedClass.Term == @class && (graph == null || mappedClass.Graph == graph)
                    select entityMapping).FirstOrDefault();
        }

        /// <inheritdoc />
        public virtual IEntityMapping FindEntityMappingFor(IEntity entity, Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!typeof(IEntity).IsAssignableFrom(type))
            {
                throw new ArgumentOutOfRangeException(nameof(type));
            }

            ICollection<ITermMappingProvider> openGenericProviders;
            if ((type.GetTypeInfo().IsGenericType) && (!type.GetTypeInfo().IsGenericTypeDefinition) &&
                (_openGenericProviders.TryGetValue(type.GetGenericTypeDefinition(), out openGenericProviders)))
            {
                _mappingBuilder.BuildMapping(_mappings, type, openGenericProviders);
            }

            return (from entityMapping in _mappings.Values
                    where entityMapping.Type == type
                    select entityMapping).FirstOrDefault();
        }

        /// <inheritdoc />
        public virtual IEntityMapping FindEntityMappingFor<TEntity>(TEntity entity) where TEntity : IEntity
        {
            return FindEntityMappingFor(entity, typeof(TEntity));
        }

        /// <inheritdoc />
        public virtual IPropertyMapping FindPropertyMappingFor(IEntity entity, Iri predicate, Iri graph = null)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return (from entityMapping in _mappings.Values
                    from propertyMapping in entityMapping.Properties
                    where propertyMapping.Term == predicate && (graph == null || propertyMapping.Graph == graph)
                    select propertyMapping).FirstOrDefault();
        }

        /// <inheritdoc />
        public virtual IPropertyMapping FindPropertyMappingFor(IEntity entity, PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            ICollection<ITermMappingProvider> openGenericProviders;
            if ((property.DeclaringType.GetTypeInfo().IsGenericType) && (!property.DeclaringType.GetTypeInfo().IsGenericTypeDefinition) &&
                (_openGenericProviders.TryGetValue(property.DeclaringType.GetGenericTypeDefinition(), out openGenericProviders)))
            {
                _mappingBuilder.BuildMapping(_mappings, property.DeclaringType, openGenericProviders);
            }

            IPropertyMapping resultCandidate = null;
            foreach (var entityMapping in _mappings.Values)
            {
                foreach (var propertyMapping in entityMapping.Properties)
                {
                    if (propertyMapping.Name != property.Name)
                    {
                        continue;
                    }

                    if ((entityMapping.Type == property.DeclaringType) || (property.DeclaringType.IsAssignableFrom(entityMapping.Type)))
                    {
                        return propertyMapping;
                    }

                    if (resultCandidate == null)
                    {
                        resultCandidate = propertyMapping;
                    }
                }
            }

            return resultCandidate;
        }

        /// <inheritdoc />
        public virtual IEnumerator<IEntityMapping> GetEnumerator()
        {
            return _mappings.Values.GetEnumerator();
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [SuppressMessage("TS0000", "NoUnitTests", Justification = "Implemented only to match the requirements. No special logic to be tested here.")]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
