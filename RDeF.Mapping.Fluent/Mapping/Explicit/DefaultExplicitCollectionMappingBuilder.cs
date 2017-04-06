using System;
using System.Reflection;
using RDeF.Entities;

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

            _mappingsBuilder.Collections[_propertyInfo] = new Tuple<Iri, Iri, CollectionStorageModel, Type>(term, graph, CollectionStorageModel.Unspecified, null);
            return new DefaultExplicitCollectionStorageModelBuilder<TEntity>(_mappingsBuilder, _propertyInfo, false);
        }

        /// <inheritdoc />
        public IExplicitCollectionStorageModelBuilder<TEntity> MappedTo(string prefix, string term, Iri graph = null)
        {
            ValidatePrefixAndTerm(prefix, term);
            _mappingsBuilder.CollectionGraphs[_propertyInfo] =
                new Tuple<string, string, Iri, CollectionStorageModel, Type>(prefix, term, graph, CollectionStorageModel.Unspecified, null);
            return new DefaultExplicitCollectionStorageModelBuilder<TEntity>(_mappingsBuilder, _propertyInfo, null);
        }

        /// <inheritdoc />
        public IExplicitCollectionStorageModelBuilder<TEntity> MappedTo(string prefix, string term, string graphPrefix, string graphTerm)
        {
            ValidatePrefixAndTerm(prefix, term);
            _mappingsBuilder.CollectionTerms[_propertyInfo] =
                new Tuple<string, string, string, string, CollectionStorageModel, Type>(prefix, term, graphPrefix, graphTerm, CollectionStorageModel.Unspecified, null);
            return new DefaultExplicitCollectionStorageModelBuilder<TEntity>(_mappingsBuilder, _propertyInfo, true);
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
