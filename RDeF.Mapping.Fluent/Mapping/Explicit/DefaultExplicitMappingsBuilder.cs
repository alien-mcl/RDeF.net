#pragma warning disable SA1402 // File may only contain a single class
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using RDeF.Entities;
using RDeF.Mapping.Providers;
using RDeF.Mapping.Reflection;
using RollerCaster.Reflection;

namespace RDeF.Mapping.Explicit
{
    /// <summary>Provides a base type for <see cref="DefaultExplicitMappingsBuilder{TEntity}" />.</summary>
    public class DefaultExplicitMappingsBuilder
    {
        internal DefaultExplicitMappingsBuilder(IEnumerable<QIriMapping> qIriMappings = null)
        {
            Classes = new List<Tuple<Iri, Iri>>();
            Properties = new Dictionary<PropertyInfo, Tuple<Iri, Iri, Type>>();
            Collections = new Dictionary<PropertyInfo, Tuple<Iri, Iri, CollectionStorageModel, Type>>();
            QIriMappings = qIriMappings ?? Array.Empty<QIriMapping>();
        }

        internal IEnumerable<QIriMapping> QIriMappings { get; set; }

        internal ICollection<Tuple<Iri, Iri>> Classes { get; }

        internal IDictionary<PropertyInfo, Tuple<Iri, Iri, Type>> Properties { get; }

        internal IDictionary<PropertyInfo, Tuple<Iri, Iri, CollectionStorageModel, Type>> Collections { get; }
    }

    /// <summary>Provides a default implementation of the <see cref="IExplicitMappingsBuilder{TEntity}" />.</summary>
    /// <typeparam name="TEntity">Type of the entity being mapped.</typeparam>
    public class DefaultExplicitMappingsBuilder<TEntity> : DefaultExplicitMappingsBuilder, IExplicitMappingsBuilder<TEntity> where TEntity : IEntity
    {
        private readonly bool _allowSingleProperty;
        private bool _isPropertyConfigured;

        internal DefaultExplicitMappingsBuilder(IEnumerable<QIriMapping> qIriMappings = null, bool allowSingleProperty = false) : base(qIriMappings)
        {
            _allowSingleProperty = allowSingleProperty;
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
            Classes.Add(new Tuple<Iri, Iri>(FluentTermMappingProvider.Resolve(null, prefix, term, QIriMappings), graph));
            return this;
        }

        /// <inheritdoc />
        public IExplicitMappingsBuilder<TEntity> MappedTo(string prefix, string term, string graphPrefix, string graphTerm)
        {
            ValidatePrefixAndTerm(prefix, term);
            Classes.Add(new Tuple<Iri, Iri>(
                FluentTermMappingProvider.Resolve(null, prefix, term, QIriMappings),
                FluentTermMappingProvider.Resolve(null, graphPrefix, graphTerm, QIriMappings)));
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
            if ((_allowSingleProperty) && (_isPropertyConfigured))
            {
                throw new InvalidOperationException("Only single property configuration is allowed.");
            }

            _isPropertyConfigured = true;
        }
    }
}
#pragma warning restore SA1402 // File may only contain a single class
