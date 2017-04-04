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
            return new DefaultExplicitCollectionStorageModelBuilder<TEntity>(_mappingsBuilder, _propertyInfo);
        }
    }
}
