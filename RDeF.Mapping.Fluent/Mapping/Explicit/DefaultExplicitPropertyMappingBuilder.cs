using System;
using System.Reflection;
using RDeF.Entities;

namespace RDeF.Mapping.Explicit
{
    /// <summary>Provides a default implementation of the <see cref="IExplicitPropertyMappingBuilder{TEntity}" />.</summary>
    /// <typeparam name="TEntity">Type of the entity being mapped.</typeparam>
    public class DefaultExplicitPropertyMappingBuilder<TEntity> : IExplicitPropertyMappingBuilder<TEntity> where TEntity : IEntity
    {
        private readonly DefaultExplicitMappingsBuilder<TEntity> _mappingsBuilder;
        private readonly PropertyInfo _propertyInfo;

        internal DefaultExplicitPropertyMappingBuilder(DefaultExplicitMappingsBuilder<TEntity> mappingsBuilder, PropertyInfo propertyInfo)
        {
            _mappingsBuilder = mappingsBuilder;
            _propertyInfo = propertyInfo;
        }

        /// <inheritdoc />
        public IExplicitValueConverterBuilder<TEntity> MappedTo(Iri term, Iri graph = null)
        {
            if (term == null)
            {
                throw new ArgumentNullException(nameof(term));
            }

            _mappingsBuilder.Properties[_propertyInfo] = new Tuple<Iri, Iri, Type>(term, graph, null);
            return new DefaultExplicitPropertyValueConverterBuilder<TEntity>(_mappingsBuilder, _propertyInfo);
        }
    }
}
