#pragma warning disable SA1402 // File may only contain a single class
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using RDeF.Entities;
using RDeF.Mapping.Reflection;
using RollerCaster.Reflection;

namespace RDeF.Mapping.Explicit
{
    /// <summary>Provides a base type for <see cref="DefaultExplicitMappingsBuilder{TEntity}" />.</summary>
    public class DefaultExplicitMappingsBuilder
    {
        internal DefaultExplicitMappingsBuilder()
        {
            Classes = new List<Tuple<Iri, Iri>>();
            ClassTerms = new List<Tuple<string, string, string, string>>();
            ClassGraphs = new List<Tuple<string, string, Iri>>();
            Properties = new Dictionary<PropertyInfo, Tuple<Iri, Iri, Type>>();
            PropertyTerms = new Dictionary<PropertyInfo, Tuple<string, string, string, string, Type>>();
            PropertyGraphs = new Dictionary<PropertyInfo, Tuple<string, string, Iri, Type>>();
            Collections = new Dictionary<PropertyInfo, Tuple<Iri, Iri, CollectionStorageModel, Type>>();
            CollectionTerms = new Dictionary<PropertyInfo, Tuple<string, string, string, string, CollectionStorageModel, Type>>();
            CollectionGraphs = new Dictionary<PropertyInfo, Tuple<string, string, Iri, CollectionStorageModel, Type>>();
        }

        internal ICollection<Tuple<Iri, Iri>> Classes { get; }

        internal ICollection<Tuple<string, string, string, string>> ClassTerms { get; }

        internal ICollection<Tuple<string, string, Iri>> ClassGraphs { get; }

        internal IDictionary<PropertyInfo, Tuple<Iri, Iri, Type>> Properties { get; }

        internal IDictionary<PropertyInfo, Tuple<string, string, string, string, Type>> PropertyTerms { get; }

        internal IDictionary<PropertyInfo, Tuple<string, string, Iri, Type>> PropertyGraphs { get; }

        internal IDictionary<PropertyInfo, Tuple<Iri, Iri, CollectionStorageModel, Type>> Collections { get; }

        internal IDictionary<PropertyInfo, Tuple<string, string, string, string, CollectionStorageModel, Type>> CollectionTerms { get; }

        internal IDictionary<PropertyInfo, Tuple<string, string, Iri, CollectionStorageModel, Type>> CollectionGraphs { get; }
    }

    /// <summary>Provides a default implementation of the <see cref="IExplicitMappingsBuilder{TEntity}" />.</summary>
    /// <typeparam name="TEntity">Type of the entity being mapped.</typeparam>
    public class DefaultExplicitMappingsBuilder<TEntity> : DefaultExplicitMappingsBuilder, IExplicitMappingsBuilder<TEntity> where TEntity : IEntity
    {
        private readonly bool _allowSinglePropertyOnly;
        private bool _isPropertyConfigured;

        internal DefaultExplicitMappingsBuilder(bool allowSinglePropertyOnly = false)
        {
            _allowSinglePropertyOnly = allowSinglePropertyOnly;
        }

        /// <inheritdoc />
        public IExplicitMappingsBuilder<TEntity> MappedTo(Iri term, Iri graph = null)
        {
            if (term == null)
            {
                throw new ArgumentNullException(nameof(term));
            }

            Classes.Add(new Tuple<Iri, Iri>(term, graph));
            return this;
        }

        /// <inheritdoc />
        public IExplicitMappingsBuilder<TEntity> MappedTo(string prefix, string term, Iri graph = null)
        {
            ValidatePrefixAndTerm(prefix, term);
            ClassGraphs.Add(new Tuple<string, string, Iri>(prefix, term, graph));
            return this;
        }

        /// <inheritdoc />
        public IExplicitMappingsBuilder<TEntity> MappedTo(string prefix, string term, string graphPrefix, string graphTerm)
        {
            ValidatePrefixAndTerm(prefix, term);
            ClassTerms.Add(new Tuple<string, string, string, string>(prefix, term, graphPrefix, graphTerm));
            return this;
        }

        /// <inheritdoc />
        public IExplicitCollectionMappingBuilder<TEntity> WithCollection<TProperty>(Expression<Func<TEntity, TProperty>> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            var propertyInfo = GetSelectedMember(collection);
            if (!propertyInfo.PropertyType.IsAnEnumerable())
            {
                throw new ArgumentOutOfRangeException(nameof(collection));
            }

            EnsureSingleProperty();
            return new DefaultExplicitCollectionMappingBuilder<TEntity>(this, propertyInfo);
        }

        /// <inheritdoc />
        public IExplicitPropertyMappingBuilder<TEntity> WithCollection(Type collectionType, string name)
        {
            if (collectionType == null)
            {
                throw new ArgumentNullException(nameof(collectionType));
            }

            if (!collectionType.IsAnEnumerable())
            {
                throw new ArgumentOutOfRangeException(nameof(collectionType));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (name.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(name));
            }

            EnsureSingleProperty();
            return new DefaultExplicitPropertyMappingBuilder<TEntity>(this, new DynamicPropertyInfo(typeof(TEntity), collectionType, name));
        }

        /// <inheritdoc />
        public IExplicitPropertyMappingBuilder<TEntity> WithProperty<TProperty>(Expression<Func<TEntity, TProperty>> property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var propertyInfo = GetSelectedMember(property);
            if (propertyInfo.PropertyType.IsAnEnumerable())
            {
                throw new ArgumentOutOfRangeException(nameof(property));
            }

            EnsureSingleProperty();
            return new DefaultExplicitPropertyMappingBuilder<TEntity>(this, propertyInfo);
        }

        /// <inheritdoc />
        public IExplicitPropertyMappingBuilder<TEntity> WithProperty(Type propertyType, string name)
        {
            if (propertyType == null)
            {
                throw new ArgumentNullException(nameof(propertyType));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (name.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(name));
            }

            EnsureSingleProperty();
            return new DefaultExplicitPropertyMappingBuilder<TEntity>(this, new DynamicPropertyInfo(typeof(TEntity), propertyType, name));
        }

        private static PropertyInfo GetSelectedMember<TMember>(Expression<Func<TEntity, TMember>> member)
        {
            var memberExpression = member.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentOutOfRangeException(nameof(member));
            }

            return (PropertyInfo)memberExpression.Member;
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

        private void EnsureSingleProperty()
        {
            if ((_allowSinglePropertyOnly) && (_isPropertyConfigured))
            {
                throw new InvalidOperationException("Only single property configuration is allowed.");
            }

            _isPropertyConfigured = true;
        }
    }
}
#pragma warning restore SA1402 // File may only contain a single class
