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

        internal DefaultExplicitCollectionStorageModelBuilder(DefaultExplicitMappingsBuilder<TEntity> mappingsBuilder, PropertyInfo propertyInfo)
        {
            _mappingsBuilder = mappingsBuilder;
            _propertyInfo = propertyInfo;
        }

        /// <inheritdoc />
        public IExplicitValueConverterBuilder<TEntity> StoredAs(CollectionStorageModel storeAs)
        {
            var current = _mappingsBuilder.Collections[_propertyInfo];
            _mappingsBuilder.Collections[_propertyInfo] = new Tuple<Iri, Iri, CollectionStorageModel, Type>(current.Item1, current.Item2, storeAs, null);
            return new DefaultExplicitCollectionValueConverterBuilder<TEntity>(_mappingsBuilder, _propertyInfo);
        }
    }
}
