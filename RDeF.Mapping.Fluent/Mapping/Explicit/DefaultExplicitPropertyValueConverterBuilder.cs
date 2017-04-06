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
        private readonly bool? _usePrefixes;

        internal DefaultExplicitPropertyValueConverterBuilder(DefaultExplicitMappingsBuilder<TEntity> mappingsBuilder, PropertyInfo propertyInfo, bool? usePrefixes)
        {
            _mappingsBuilder = mappingsBuilder;
            _propertyInfo = propertyInfo;
            _usePrefixes = usePrefixes;
        }

        /// <inheritdoc />
        public IExplicitMappingsBuilder<TEntity> WithValueConverter<TConverter>() where TConverter : ILiteralConverter
        {
            if (_usePrefixes == null)
            {
                var current = _mappingsBuilder.PropertyGraphs[_propertyInfo];
                _mappingsBuilder.PropertyGraphs[_propertyInfo] = new Tuple<string, string, Iri, Type>(current.Item1, current.Item2, current.Item3, typeof(TConverter));
            }
            else if (_usePrefixes.Value)
            {
                var current = _mappingsBuilder.PropertyTerms[_propertyInfo];
                _mappingsBuilder.PropertyTerms[_propertyInfo] = new Tuple<string, string, string, string, Type>(current.Item1, current.Item2, current.Item3, current.Item4, typeof(TConverter));
            }
            else
            {
                var current = _mappingsBuilder.Properties[_propertyInfo];
                _mappingsBuilder.Properties[_propertyInfo] = new Tuple<Iri, Iri, Type>(current.Item1, current.Item2, typeof(TConverter));
            }

            return _mappingsBuilder;
        }

        /// <inheritdoc />
        public IExplicitMappingsBuilder<TEntity> WithDefaultConverter()
        {
            return _mappingsBuilder;
        }
    }
}
