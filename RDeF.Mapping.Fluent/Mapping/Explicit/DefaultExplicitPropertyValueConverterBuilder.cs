using System;
using System.Reflection;
using RDeF.Entities;

namespace RDeF.Mapping.Explicit
{
    /// <summary>Provides a default property related implementation of the <see cref="IExplicitValueConverterBuilder{TEntity}" />.</summary>
    /// <typeparam name="TEntity">Type of the entity being mapped.</typeparam>
    public class DefaultExplicitPropertyValueConverterBuilder<TEntity> : IExplicitValueConverterBuilder<TEntity> where TEntity : IEntity
    {
        private readonly DefaultExplicitMappingsBuilder<TEntity> _mappingsBuilder;
        private readonly PropertyInfo _propertyInfo;

        internal DefaultExplicitPropertyValueConverterBuilder(DefaultExplicitMappingsBuilder<TEntity> mappingsBuilder, PropertyInfo propertyInfo)
        {
            _mappingsBuilder = mappingsBuilder;
            _propertyInfo = propertyInfo;
        }

        /// <inheritdoc />
        public IExplicitMappingsBuilder<TEntity> WithValueConverter<TConverter>() where TConverter : ILiteralConverter
        {
            var current = _mappingsBuilder.Properties[_propertyInfo];
            _mappingsBuilder.Properties[_propertyInfo] = new Tuple<Iri, Iri, Type>(current.Item1, current.Item2, typeof(TConverter));
            return _mappingsBuilder;
        }

        /// <inheritdoc />
        public IExplicitMappingsBuilder<TEntity> WithDefaultConverter()
        {
            return _mappingsBuilder;
        }
    }
}
