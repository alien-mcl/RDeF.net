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
            if (mappingsBuilder == null)
            {
                return entity;
            }

            var builder = new DefaultExplicitMappingsBuilder<TEntity>();
            mappingsBuilder(builder);
            var mappingProviders = new List<ITermMappingProvider>();
            mappingProviders.AddClasses(builder);
            mappingProviders.AddCollections(builder);
            mappingProviders.AddProperties(builder);
            ExplicitMappings[entityContext].Set(mappingProviders.BuildMapping<TEntity>());
            return entity;
        }

        private static void AddClasses<TEntity>(this ICollection<ITermMappingProvider> mappingProviders, DefaultExplicitMappingsBuilder<TEntity> builder) where TEntity : IEntity
        {
            foreach (var @class in builder.Classes)
            {
                mappingProviders.Add(new FluentEntityMappingProvider(typeof(TEntity), @class.Item1, @class.Item2));
            }
        }

        private static void AddCollections<TEntity>(this ICollection<ITermMappingProvider> mappingProviders, DefaultExplicitMappingsBuilder<TEntity> builder) where TEntity : IEntity
        {
            foreach (var collection in builder.Collections)
            {
                var collectionMappingProvider = new FluentCollectionMappingProvider(
                    typeof(TEntity),
                    collection.Key,
                    collection.Value.Item1,
                    collection.Value.Item3,
                    collection.Value.Item4,
                    collection.Value.Item2);
                mappingProviders.Add(collectionMappingProvider);
            }
        }

        private static void AddProperties<TEntity>(this ICollection<ITermMappingProvider> mappingProviders, DefaultExplicitMappingsBuilder<TEntity> builder) where TEntity : IEntity
        {
            foreach (var collection in builder.Properties)
            {
                var collectionMappingProvider = new FluentPropertyMappingProvider(
                    typeof(TEntity),
                    collection.Key,
                    collection.Value.Item1,
                    collection.Value.Item3,
                    collection.Value.Item2);
                mappingProviders.Add(collectionMappingProvider);
            }
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
                    entityMapping.Properties.Add(new CollectionMapping(
                        entityMapping,
                        collectionMappingProvider.Property.Name,
                        collectionMappingProvider.GetGraph(QIriMappings),
                        collectionMappingProvider.GetTerm(QIriMappings),
                        LiteralConverters.First(converter => converter.GetType() == collectionMappingProvider.ValueConverterType),
                        collectionMappingProvider.StoreAs));
                    continue;
                }

                var propertyMappingProvider = mappingProvider as IPropertyMappingProvider;
                if (propertyMappingProvider != null)
                {
                    entityMapping.Properties.Add(new PropertyMapping(
                        entityMapping,
                        propertyMappingProvider.Property.Name,
                        propertyMappingProvider.GetGraph(QIriMappings),
                        propertyMappingProvider.GetTerm(QIriMappings),
                        LiteralConverters.First(converter => converter.GetType() == propertyMappingProvider.ValueConverterType)));
                }
            }

            return entityMapping;
        }
    }
}
