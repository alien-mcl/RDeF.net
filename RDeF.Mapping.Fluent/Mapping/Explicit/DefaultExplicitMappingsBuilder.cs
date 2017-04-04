using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using RDeF.Entities;
using RollerCaster.Reflection;

namespace RDeF.Mapping.Explicit
{
    /// <summary>Provides a default implementation of the <see cref="IExplicitMappingsBuilder{TEntity}" />.</summary>
    /// <typeparam name="TEntity">Type of the entity being mapped.</typeparam>
    public class DefaultExplicitMappingsBuilder<TEntity> : IExplicitMappingsBuilder<TEntity> where TEntity : IEntity
    {
        internal DefaultExplicitMappingsBuilder()
        {
            Classes = new List<Tuple<Iri, Iri>>();
            Properties = new Dictionary<PropertyInfo, Tuple<Iri, Iri, Type>>();
            Collections = new Dictionary<PropertyInfo, Tuple<Iri, Iri, CollectionStorageModel, Type>>();
        }

        internal ICollection<Tuple<Iri, Iri>> Classes { get; }

        internal IDictionary<PropertyInfo, Tuple<Iri, Iri, Type>> Properties { get; }

        internal IDictionary<PropertyInfo, Tuple<Iri, Iri, CollectionStorageModel, Type>> Collections { get; }

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

            return new DefaultExplicitCollectionMappingBuilder<TEntity>(this, propertyInfo);
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

            return new DefaultExplicitPropertyMappingBuilder<TEntity>(this, propertyInfo);
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
    }
}
