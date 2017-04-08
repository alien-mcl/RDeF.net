using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using RDeF.Entities;
using RDeF.Mapping.Explicit;
using RDeF.Mapping.Providers;
using RDeF.Mapping.Visitors;

namespace RDeF.Mapping.Entities
{
    /// <summary>Provides useful extensions for <see cref="IEntityContext" />.</summary>
    public static class EntityContextExtensions
    {
        internal static readonly IDictionary<IEntityContext, IExplicitMappings> ExplicitMappings = new ConcurrentDictionary<IEntityContext, IExplicitMappings>();

        internal static IEnumerable<ILiteralConverter> LiteralConverters { get; set; }

        internal static IEnumerable<IMappingProviderVisitor> MappingVisitors { get; set; }

        internal static IEnumerable<QIriMapping> QIriMappings { get; set; }

        /// <summary>Creates a specified entity.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entityContext">Entity context to work with.</param>
        /// <param name="iri">The identifier of the entity to be loaded.</param>
        /// <param name="mappingsBuilder">Explicit mapping builder.</param>
        /// <returns>Instance of the entity of a given <paramref name="iri" />.</returns>
        public static TEntity Create<TEntity>(this IEntityContext entityContext, Iri iri, Action<IExplicitMappingsBuilder<TEntity>> mappingsBuilder) where TEntity : IEntity
        {
            if (entityContext == null)
            {
                throw new ArgumentNullException(nameof(entityContext));
            }

            var entity = entityContext.Create<TEntity>(iri);
            if ((mappingsBuilder != null) && (ExplicitMappings.ContainsKey(entityContext)))
            {
                entityContext.BuildExplicitMappings(mappingsBuilder);
            }

            return entity;
        }

        /// <summary>Loads a specified entity.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entityContext">Entity context to work with.</param>
        /// <param name="iri">The identifier of the entity to be loaded.</param>
        /// <param name="mappingsBuilder">Explicit mapping builder.</param>
        /// <returns>Instance of the entity of a given <paramref name="iri" />.</returns>
        public static TEntity Load<TEntity>(this IEntityContext entityContext, Iri iri, Action<IExplicitMappingsBuilder<TEntity>> mappingsBuilder) where TEntity : IEntity
        {
            if (entityContext == null)
            {
                throw new ArgumentNullException(nameof(entityContext));
            }

            var entity = entityContext.Load<TEntity>(iri);
            if ((mappingsBuilder != null) && (ExplicitMappings.ContainsKey(entityContext)))
            {
                entityContext.BuildExplicitMappings(mappingsBuilder);
            }

            return entity;
        }

        internal static void AddClasses(this ICollection<ITermMappingProvider> mappingProviders, Type entityType, DefaultExplicitMappingsBuilder builder)
        {
            foreach (var @class in builder.Classes)
            {
                mappingProviders.Add(new FluentEntityMappingProvider(entityType, @class.Item1, @class.Item2));
            }

            foreach (var @class in builder.ClassTerms)
            {
                mappingProviders.Add(new FluentEntityMappingProvider(entityType, @class.Item1, @class.Item2, @class.Item3, @class.Item4));
            }

            foreach (var @class in builder.ClassGraphs)
            {
                mappingProviders.Add(new FluentEntityMappingProvider(entityType, @class.Item1, @class.Item2, @class.Item3));
            }
        }

        internal static void AddCollections(this ICollection<ITermMappingProvider> mappingProviders, Type entityType, DefaultExplicitMappingsBuilder builder)
        {
            foreach (var collection in builder.Collections)
            {
                var collectionMappingProvider = new FluentCollectionMappingProvider(
                    entityType,
                    collection.Key,
                    collection.Value.Item1,
                    collection.Value.Item3,
                    collection.Value.Item4,
                    collection.Value.Item2);
                mappingProviders.Add(collectionMappingProvider);
            }

            foreach (var collection in builder.CollectionTerms)
            {
                var collectionMappingProvider = new FluentCollectionMappingProvider(
                    entityType,
                    collection.Key,
                    collection.Value.Item1,
                    collection.Value.Item2,
                    collection.Value.Item5,
                    collection.Value.Item6,
                    collection.Value.Item3,
                    collection.Value.Item4);
                mappingProviders.Add(collectionMappingProvider);
            }

            foreach (var collection in builder.CollectionGraphs)
            {
                var collectionMappingProvider = new FluentCollectionMappingProvider(
                    entityType,
                    collection.Key,
                    collection.Value.Item1,
                    collection.Value.Item2,
                    collection.Value.Item4,
                    collection.Value.Item5,
                    collection.Value.Item3);
                mappingProviders.Add(collectionMappingProvider);
            }
        }

        internal static void AddProperties(this ICollection<ITermMappingProvider> mappingProviders, Type entityType, DefaultExplicitMappingsBuilder builder)
        {
            foreach (var property in builder.Properties)
            {
                var propertyMappingProvider = new FluentPropertyMappingProvider(
                    entityType,
                    property.Key,
                    property.Value.Item1,
                    property.Value.Item3,
                    property.Value.Item2);
                mappingProviders.Add(propertyMappingProvider);
            }

            foreach (var property in builder.PropertyTerms)
            {
                var propertyMappingProvider = new FluentPropertyMappingProvider(
                    entityType,
                    property.Key,
                    property.Value.Item1,
                    property.Value.Item2,
                    property.Value.Item5,
                    property.Value.Item3,
                    property.Value.Item4);
                mappingProviders.Add(propertyMappingProvider);
            }

            foreach (var property in builder.PropertyGraphs)
            {
                var propertyMappingProvider = new FluentPropertyMappingProvider(
                    entityType,
                    property.Key,
                    property.Value.Item1,
                    property.Value.Item2,
                    property.Value.Item4,
                    property.Value.Item3);
                mappingProviders.Add(propertyMappingProvider);
            }
        }

        private static void BuildExplicitMappings<TEntity>(this IEntityContext entityContext, Action<IExplicitMappingsBuilder<TEntity>> mappingsBuilder) where TEntity : IEntity
        {
            var builder = new DefaultExplicitMappingsBuilder<TEntity>();
            mappingsBuilder(builder);
            var mappingProviders = new List<ITermMappingProvider>();
            mappingProviders.AddClasses(builder);
            mappingProviders.AddCollections(builder);
            mappingProviders.AddProperties(builder);
            ExplicitMappings[entityContext].Set(mappingProviders.BuildMapping<TEntity>());
        }

        private static void AddClasses<TEntity>(this ICollection<ITermMappingProvider> mappingProviders, DefaultExplicitMappingsBuilder<TEntity> builder) where TEntity : IEntity
        {
            mappingProviders.AddClasses(typeof(TEntity), builder);
        }

        private static void AddCollections<TEntity>(this ICollection<ITermMappingProvider> mappingProviders, DefaultExplicitMappingsBuilder<TEntity> builder) where TEntity : IEntity
        {
            mappingProviders.AddCollections(typeof(TEntity), builder);
        }

        private static void AddProperties<TEntity>(this ICollection<ITermMappingProvider> mappingProviders, DefaultExplicitMappingsBuilder<TEntity> builder) where TEntity : IEntity
        {
            mappingProviders.AddProperties(typeof(TEntity), builder);
        }

        private static IEntityMapping BuildMapping<TEntity>(this ICollection<ITermMappingProvider> mappingProviders)
        {
            var entityMapping = new MergingEntityMapping(typeof(TEntity));
            foreach (var mappingProvider in mappingProviders)
            {
                mappingProvider.Accept(MappingVisitors);
                var entityMappingProvider = mappingProvider as IEntityMappingProvider;
                if (entityMappingProvider != null)
                {
                    entityMapping.Classes.Add(new StatementMapping(entityMappingProvider.GetGraph(QIriMappings), entityMappingProvider.GetTerm(QIriMappings)));
                    continue;
                }

                var collectionMappingProvider = mappingProvider as ICollectionMappingProvider;
                if (collectionMappingProvider != null)
                {
                    entityMapping.Properties.Add(CollectionMapping.CreateFrom(
                        entityMapping,
                        collectionMappingProvider,
                        LiteralConverters.First(converter => converter.GetType() == collectionMappingProvider.ValueConverterType),
                        QIriMappings));
                    continue;
                }

                var propertyMappingProvider = mappingProvider as IPropertyMappingProvider;
                if (propertyMappingProvider != null)
                {
                    entityMapping.Properties.Add(PropertyMapping.CreateFrom(
                        entityMapping,
                        propertyMappingProvider,
                        LiteralConverters.First(converter => converter.GetType() == propertyMappingProvider.ValueConverterType),
                        QIriMappings));
                }
            }

            return entityMapping;
        }
    }
}
