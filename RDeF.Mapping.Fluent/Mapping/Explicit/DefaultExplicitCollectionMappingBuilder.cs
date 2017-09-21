using System;
using System.Reflection;
using RDeF.Entities;
using RDeF.Mapping.Providers;
using RDeF.Mapping.Reflection;

namespace RDeF.Mapping.Explicit
{
    /// <summary>Provides a default implementation of the <see cref="IExplicitPropertyMappingBuilder{TEntity}" />.</summary>
    /// <typeparam name="TEntity">Type of the entity being mapped.</typeparam>
    public class DefaultExplicitCollectionMappingBuilder<TEntity> : IExplicitCollectionMappingBuilder<TEntity> where TEntity : IEntity
    {
        private readonly DefaultExplicitMappingsBuilder<TEntity> _mappingsBuilder;
        private readonly PropertyInfo _propertyInfo;

        internal DefaultExplicitCollectionMappingBuilder(DefaultExplicitMappingsBuilder<TEntity> mappingsBuilder, PropertyInfo propertyInfo)
        {
            _mappingsBuilder = mappingsBuilder;
            _propertyInfo = propertyInfo;
        }

        /// <inheritdoc />
        public IExplicitCollectionStorageModelBuilder<TEntity> MappedTo(Iri term, Iri graph = null)
        {
            if (term == null)
            {
                throw new ArgumentNullException(nameof(term));
            }

            var propertyInfo = new ExplicitlyMappedPropertyInfo(_propertyInfo, term, graph);
            _mappingsBuilder.Collections[propertyInfo] = new Tuple<Iri, Iri, CollectionStorageModel, Type>(term, graph, CollectionStorageModel.Unspecified, null);
            return new DefaultExplicitCollectionStorageModelBuilder<TEntity>(_mappingsBuilder, propertyInfo);
        }

        /// <inheritdoc />
        public IExplicitCollectionStorageModelBuilder<TEntity> MappedTo(string prefix, string term, Iri graph = null)
        {
            ValidatePrefixAndTerm(prefix, term);
            return MappedTo(FluentTermMappingProvider.Resolve(null, prefix, term, _mappingsBuilder.QIriMappings), graph);
        }

        /// <inheritdoc />
        public IExplicitCollectionStorageModelBuilder<TEntity> MappedTo(string prefix, string term, string graphPrefix, string graphTerm)
        {
            ValidatePrefixAndTerm(prefix, term);
            return MappedTo(
                FluentTermMappingProvider.Resolve(null, prefix, term, _mappingsBuilder.QIriMappings),
                FluentTermMappingProvider.Resolve(null, graphPrefix, graphTerm, _mappingsBuilder.QIriMappings));
        }

        private static void ValidatePrefixAndTerm(string prefix, string term)
        {
            if (prefix == null)
            {
                throw new ArgumentNullException(nameof(prefix));
            }

            if (prefix.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(prefix));
            }

            if (term == null)
            {
                throw new ArgumentNullException(nameof(term));
            }

            if (term.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(term));
            }
        }
    }
}
