using System;
using System.Reflection;
using RDeF.Entities;

namespace RDeF.Mapping.Explicit
{
    /// <summary>Provides a default implementation of the <see cref="IExplicitCollectionStorageModelBuilder{TEntity}" />.</summary>
    /// <typeparam name="TEntity">Type of the entity being mapped.</typeparam>
    public class DefaultExplicitCollectionStorageModelBuilder<TEntity> : IExplicitCollectionStorageModelBuilder<TEntity> where TEntity : IEntity
    {
        private readonly DefaultExplicitMappingsBuilder<TEntity> _mappingsBuilder;
        private readonly PropertyInfo _propertyInfo;
        private readonly bool? _usePrefixes;

        internal DefaultExplicitCollectionStorageModelBuilder(DefaultExplicitMappingsBuilder<TEntity> mappingsBuilder, PropertyInfo propertyInfo, bool? usePrefixes)
        {
            _mappingsBuilder = mappingsBuilder;
            _propertyInfo = propertyInfo;
            _usePrefixes = usePrefixes;
        }

        /// <inheritdoc />
        public IExplicitValueConverterBuilder<TEntity> StoredAs(CollectionStorageModel storeAs)
        {
            if (_usePrefixes == null)
            {
                var current = _mappingsBuilder.CollectionGraphs[_propertyInfo];
                _mappingsBuilder.CollectionGraphs[_propertyInfo] =
                    new Tuple<string, string, Iri, CollectionStorageModel, Type>(current.Item1, current.Item2, current.Item3, storeAs, null);
            }
            else if (_usePrefixes.Value)
            {
                var current = _mappingsBuilder.CollectionTerms[_propertyInfo];
                _mappingsBuilder.CollectionTerms[_propertyInfo] =
                    new Tuple<string, string, string, string, CollectionStorageModel, Type>(current.Item1, current.Item2, current.Item3, current.Item4, storeAs, null);
            }
            else
            {
                var current = _mappingsBuilder.Collections[_propertyInfo];
                _mappingsBuilder.Collections[_propertyInfo] = new Tuple<Iri, Iri, CollectionStorageModel, Type>(current.Item1, current.Item2, storeAs, null);
            }

            return new DefaultExplicitCollectionValueConverterBuilder<TEntity>(_mappingsBuilder, _propertyInfo, _usePrefixes);
        }
    }
}
