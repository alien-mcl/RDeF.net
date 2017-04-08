using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using RDeF.Collections;
using RDeF.Mapping.Providers;
using RDeF.Mapping.Visitors;

namespace RDeF.Mapping
{
    internal class DefaultMappingBuilder : IMappingBuilder
    {
        private readonly IConverterProvider _converterProvider;
        private readonly IEnumerable<QIriMapping> _qiriMappings;
        private readonly IEnumerable<IMappingProviderVisitor> _mappingProviderVisitors;

        /// <summary>Initializes a new instance of the <see cref="DefaultMappingBuilder" /> class.</summary>
        /// <param name="converterProvider">Converters provider.</param>
        /// <param name="qiriMappings">QIri mappings.</param>
        /// <param name="mappingProviderVisitors">Mapping provider visitors.</param>
        public DefaultMappingBuilder(IConverterProvider converterProvider, IEnumerable<QIriMapping> qiriMappings, IEnumerable<IMappingProviderVisitor> mappingProviderVisitors)
        {
            _converterProvider = converterProvider;
            _qiriMappings = qiriMappings;
            _mappingProviderVisitors = mappingProviderVisitors;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "Class is used internally only.")]
        public void BuildMapping(IDictionary<Type, IEntityMapping> mappings, Type closedGenericType, IEnumerable<ITermMappingProvider> openGenericEntityMappingProviders)
        {
            IEntityMapping entityMapping;
            if (mappings.TryGetValue(closedGenericType, out entityMapping))
            {
                return;
            }

            foreach (var openGenericMappingProvider in openGenericEntityMappingProviders)
            {
                ITermMappingProvider mappingProvider = openGenericMappingProvider.TryCloseGenericTermMappingProvider(closedGenericType);
                if (mappingProvider is ClosedGenericTermMappingProvider)
                {
                    mappingProvider.Accept(_mappingProviderVisitors);
                }

                var existingEntityMapping = BuildEntityMapping(mappings, mappingProvider);
                BuildPropertyMapping(existingEntityMapping, mappingProvider as IPropertyMappingProvider);
            }
        }

        /// <inheritdoc />
        public IDictionary<Type, IEntityMapping> BuildMappings(
            IEnumerable<IMappingSource> mappingSources,
            IDictionary<Type, ICollection<ITermMappingProvider>> openGenericMappingProviders)
        {
            var mappings = new ConcurrentDictionary<Type, IEntityMapping>();
            foreach (var mappingProvider in mappingSources.SelectMany(mappingSource => mappingSource.GatherEntityMappingProviders()).WithInheritance())
            {
                mappingProvider.Accept(_mappingProviderVisitors);
                if (mappingProvider.EntityType.GetTypeInfo().IsGenericTypeDefinition)
                {
                    openGenericMappingProviders.EnsureKey(mappingProvider.EntityType).Add(mappingProvider);
                    continue;
                }

                var existingEntityMapping = BuildEntityMapping(mappings, mappingProvider);
                BuildPropertyMapping(existingEntityMapping, mappingProvider as IPropertyMappingProvider);
            }

            return mappings;
        }

        private MergingEntityMapping BuildEntityMapping(IDictionary<Type, IEntityMapping> mappings, ITermMappingProvider mappingProvider)
        {
            IEntityMapping existingEntityMapping;
            if (!mappings.TryGetValue(mappingProvider.EntityType, out existingEntityMapping))
            {
                mappings[mappingProvider.EntityType] = existingEntityMapping = new MergingEntityMapping(mappingProvider.EntityType);
            }

            var mergingEntityMapping = existingEntityMapping as MergingEntityMapping;
            var entityMappingProvider = mappingProvider as IEntityMappingProvider;
            if (entityMappingProvider == null)
            {
                return mergingEntityMapping;
            }

            var term = entityMappingProvider.GetTerm(_qiriMappings);
            if (term != null)
            {
                mergingEntityMapping.Classes.Add(new StatementMapping(entityMappingProvider.GetGraph(_qiriMappings), term));
            }

            return mergingEntityMapping;
        }

        private void BuildPropertyMapping(MergingEntityMapping existingEntityMapping, IPropertyMappingProvider propertyMappingProvider)
        {
            if (propertyMappingProvider == null)
            {
                return;
            }

            IConverter valueConverter = null;
            if (propertyMappingProvider.ValueConverterType != null)
            {
                valueConverter = _converterProvider.FindConverter(propertyMappingProvider.ValueConverterType);
            }

            var collectionMappingProvider = propertyMappingProvider as ICollectionMappingProvider;
            IPropertyMapping propertyMapping = collectionMappingProvider != null
                ? CollectionMapping.CreateFrom(existingEntityMapping, collectionMappingProvider, valueConverter, _qiriMappings)
                : PropertyMapping.CreateFrom(existingEntityMapping, propertyMappingProvider, valueConverter, _qiriMappings);
            var existingPropertyMapping = existingEntityMapping.Properties.FirstOrDefault(mapping => mapping.Name == propertyMappingProvider.Property.Name);
            if (existingPropertyMapping != null)
            {
                if (!PropertyMappingEqualityComparer.Default.Equals(propertyMapping, existingPropertyMapping))
                {
                    throw new AmbiguousMappingException(
                        $"Mapping for ${propertyMappingProvider.Property.Name} for type ${existingEntityMapping.Type} is already defined.");
                }

                return;
            }

            existingEntityMapping.Properties.Add(propertyMapping);
        }
    }
}
