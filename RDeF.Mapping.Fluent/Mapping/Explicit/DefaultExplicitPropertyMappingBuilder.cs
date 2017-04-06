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
            return new DefaultExplicitPropertyValueConverterBuilder<TEntity>(_mappingsBuilder, _propertyInfo, false);
        }

        /// <inheritdoc />
        public IExplicitValueConverterBuilder<TEntity> MappedTo(string prefix, string term, Iri graph = null)
        {
            ValidatePrefixAndTerm(prefix, term);
            _mappingsBuilder.PropertyGraphs[_propertyInfo] = new Tuple<string, string, Iri, Type>(prefix, term, graph, null);
            return new DefaultExplicitPropertyValueConverterBuilder<TEntity>(_mappingsBuilder, _propertyInfo, null);
        }

        /// <inheritdoc />
        public IExplicitValueConverterBuilder<TEntity> MappedTo(string prefix, string term, string graphPrefix, string graphTerm)
        {
            ValidatePrefixAndTerm(prefix, term);
            _mappingsBuilder.PropertyTerms[_propertyInfo] = new Tuple<string, string, string, string, Type>(prefix, term, graphPrefix, graphTerm, null);
            return new DefaultExplicitPropertyValueConverterBuilder<TEntity>(_mappingsBuilder, _propertyInfo, true);
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
