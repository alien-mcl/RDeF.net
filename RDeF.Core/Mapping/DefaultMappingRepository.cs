using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using RDeF.Entities;
using RDeF.Mapping.Providers;
using RDeF.Mapping.Visitors;

namespace RDeF.Mapping
{
    /// <summary>Provides a default implementation of the <see cref="IMappingsRepository" /> which gathers mapping from various sources.</summary>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "The type is not considered a collection.")]
    public sealed class DefaultMappingRepository : IMappingsRepository
    {
        /// <summary>Initializes a new instance of the <see cref="DefaultMappingRepository"/> class.</summary>
        /// <param name="mappingSources">Collection of mapping sources.</param>
        /// <param name="mappingProviderVisitors">Mapping provider visitors.</param>
        /// <param name="converters">Possible converters.</param>
        /// <param name="qiriMappings">QIri mappings.</param>
        public DefaultMappingRepository(
            IEnumerable<IMappingSource> mappingSources,
            IEnumerable<IMappingProviderVisitor> mappingProviderVisitors,
            IEnumerable<ILiteralConverter> converters,
            IEnumerable<QIriMapping> qiriMappings)
        {
            if (mappingProviderVisitors == null)
            {
                throw new ArgumentNullException(nameof(mappingProviderVisitors));
            }

            if (converters == null)
            {
                throw new ArgumentNullException(nameof(converters));
            }

            if (qiriMappings == null)
            {
                throw new ArgumentNullException(nameof(qiriMappings));
            }

            MappingProviderVisitors = mappingProviderVisitors;
            Converters = converters;
            QIriMappings = qiriMappings;
            Mappings = new ConcurrentDictionary<Type, MergingEntityMapping>();
            BuildMappings(mappingSources);
        }

        internal ConcurrentDictionary<Type, MergingEntityMapping> Mappings { get; }

        internal IEnumerable<IMappingProviderVisitor> MappingProviderVisitors { get; }

        internal IEnumerable<IConverter> Converters { get; }

        internal IEnumerable<QIriMapping> QIriMappings { get; }

        /// <inheritdoc />
        public IEntityMapping FindEntityMappingFor(Iri @class, Iri graph = null)
        {
            if (@class == null)
            {
                throw new ArgumentNullException(nameof(@class));
            }

            return (from entityMapping in Mappings.Values
                    from mappedClass in entityMapping.Classes
                    where mappedClass.Term == @class && (graph == null || mappedClass.Graph == graph)
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
        public IPropertyMapping FindPropertyMappingFor(Iri predicate, Iri graph = null)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return (from entityMapping in Mappings.Values
                    from propertyMapping in entityMapping.Properties
                    where propertyMapping.Term == predicate && (graph == null || propertyMapping.Graph == graph)
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
        [SuppressMessage("TS0000", "NoUnitTests", Justification = "Implemented only to match the requirements. No special logic to be tested here.")]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void BuildMappings(IEnumerable<IMappingSource> mappingSources)
        {
            lock (Mappings)
            {
                foreach (var mappingProvider in mappingSources.SelectMany(mappingSource => mappingSource.GatherEntityMappingProviders()))
                {
                    mappingProvider.Visit(MappingProviderVisitors);
                    MergingEntityMapping existingEntityMapping = BuildEntityMapping(mappingProvider);
                    BuildPropertyMapping(existingEntityMapping, mappingProvider as IPropertyMappingProvider);
                }
            }
        }

        private MergingEntityMapping BuildEntityMapping(ITermMappingProvider mappingProvider)
        {
            MergingEntityMapping existingEntityMapping;
            if (!Mappings.TryGetValue(mappingProvider.EntityType, out existingEntityMapping))
            {
                Mappings[mappingProvider.EntityType] = existingEntityMapping = new MergingEntityMapping(mappingProvider.EntityType);
            }

            var entityMappingProvider = mappingProvider as IEntityMappingProvider;
            if (entityMappingProvider == null)
            {
                return existingEntityMapping;
            }

            var term = entityMappingProvider.GetTerm(QIriMappings);
            if (term != null)
            {
                existingEntityMapping.Classes.Add(new StatementMapping(entityMappingProvider.GetGraph(QIriMappings), term));
            }

            return existingEntityMapping;
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
                valueConverter = (from converter in Converters
                                  let match = propertyMappingProvider.ValueConverterType == converter.GetType() ? 2 :
                                      (propertyMappingProvider.ValueConverterType.IsInstanceOfType(converter) ? 1 : 0)
                                  where match > 0
                                  orderby match descending
                                  select converter).First();
            }

            IPropertyMapping propertyMapping;
            var collectionMapping = propertyMappingProvider as ICollectionMappingProvider;
            if (collectionMapping != null)
            {
                propertyMapping = new CollectionMapping(
                    existingEntityMapping,
                    collectionMapping.Property.Name,
                    collectionMapping.GetGraph(QIriMappings),
                    collectionMapping.GetTerm(QIriMappings),
                    valueConverter,
                    collectionMapping.StoreAs);
            }
            else
            {
                propertyMapping = new PropertyMapping(
                    existingEntityMapping,
                    propertyMappingProvider.Property.Name,
                    propertyMappingProvider.GetGraph(QIriMappings),
                    propertyMappingProvider.GetTerm(QIriMappings),
                    valueConverter);
            }

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
