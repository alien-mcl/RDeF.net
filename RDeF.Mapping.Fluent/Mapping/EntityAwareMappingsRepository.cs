using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using RDeF.Entities;
using RDeF.Mapping.Entities;
using RDeF.Mapping.Explicit;

namespace RDeF.Mapping
{
    /// <summary>Provides a default implementation of the <see cref="IMappingsRepository" /> that is aware of <see cref="IEntity" /> owned mappings.</summary>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "This is not supposed to be a collection - IEnumerable implementation is only for convinience of use.")]
    public class EntityAwareMappingsRepository : IMappingsRepository
    {
        private readonly DefaultMappingsRepository _mappingsRepository;
        private readonly Func<IEntityContext> _entityContext;

        /// <summary>Initializes a new instance of the <see cref="EntityAwareMappingsRepository" /> class.</summary>
        /// <param name="entityContext">Entity context provider.</param>
        /// <param name="mappingsRepository">Mapping repository.</param>
        public EntityAwareMappingsRepository(Func<IEntityContext> entityContext, DefaultMappingsRepository mappingsRepository)
        {
            if (entityContext == null)
            {
                throw new ArgumentNullException(nameof(entityContext));
            }

            _entityContext = entityContext;
            _mappingsRepository = mappingsRepository;
        }

        /// <inheritdoc />
        public IEntityMapping FindEntityMappingFor(IEntity entity, Iri @class, Iri graph = null)
        {
            return _mappingsRepository.FindEntityMappingFor(entity, @class, graph);
        }

        /// <inheritdoc />
        public IEntityMapping FindEntityMappingFor(IEntity entity, Type type)
        {
            IEntityMapping result = null;
            if (entity != null)
            {
                var explicitMappings = GetExplicitMappingsFor(_entityContext());
                if (explicitMappings != null)
                {
                    result = explicitMappings.FindEntityMappingFor(type, entity.Iri);
                }
            }

            return result ?? _mappingsRepository.FindEntityMappingFor(entity, type);
        }

        /// <inheritdoc />
        public IEntityMapping FindEntityMappingFor<TEntity>(TEntity entity) where TEntity : IEntity
        {
            return FindEntityMappingFor(entity, typeof(TEntity));
        }

        /// <inheritdoc />
        public IEnumerable<IPropertyMapping> FindPropertyMappingsFor(IEntity entity, Iri predicate, Iri graph = null)
        {
            IEnumerable<IPropertyMapping> result = Array.Empty<IPropertyMapping>();
            if (entity != null)
            {
                var explicitMappings = GetExplicitMappingsFor(_entityContext());
                if (explicitMappings != null)
                {
                    result = explicitMappings.FindPropertyMappingsFor(predicate, graph, entity.Iri);
                }
            }

            return result.Any() ? result : _mappingsRepository.FindPropertyMappingsFor(entity, predicate, graph);
        }

        /// <inheritdoc />
        public IPropertyMapping FindPropertyMappingFor(IEntity entity, Iri predicate, Iri graph = null)
        {
            return FindPropertyMappingsFor(entity, predicate, graph).FirstOrDefault();
        }

        /// <inheritdoc />
        public IPropertyMapping FindPropertyMappingFor(IEntity entity, PropertyInfo property)
        {
            IPropertyMapping result = null;
            if (entity != null)
            {
                var explicitMappings = GetExplicitMappingsFor(_entityContext());
                if (explicitMappings != null)
                {
                    result = explicitMappings.FindPropertyMappingFor(property, entity.Iri);
                }
            }

            return result ?? _mappingsRepository.FindPropertyMappingFor(entity, property);
        }

        /// <inheritdoc />
        public IEnumerator<IEntityMapping> GetEnumerator()
        {
            var explicitMappings = GetExplicitMappingsFor(_entityContext());
            if (explicitMappings != null)
            {
                return new JoinEnumerator(explicitMappings.GetEnumerator(), _mappingsRepository.GetEnumerator());
            }

            return _mappingsRepository.GetEnumerator();
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [SuppressMessage("TS0000", "NoUnitTests", Justification = "Implemented only to match the requirements. No special logic to be tested here.")]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static IExplicitMappings GetExplicitMappingsFor(IEntityContext entityContext)
        {
            IExplicitMappings explicitMappings;
            if ((entityContext == null) || (!EntityContextExtensions.ExplicitMappings.TryGetValue(entityContext, out explicitMappings)))
            {
                return null;
            }

            return explicitMappings;
        }

        internal class JoinEnumerator : IEnumerator<IEntityMapping>
        {
            private readonly IEnumerator<IEntityMapping> _primaryEnumerator;
            private readonly IEnumerator<IEntityMapping> _secondaryEnumerator;
            private IEnumerator<IEntityMapping> _currentEnumerator;

            internal JoinEnumerator(IEnumerator<IEntityMapping> primaryEnumerator, IEnumerator<IEntityMapping> secondaryEnumerator)
            {
                _currentEnumerator = _primaryEnumerator = primaryEnumerator;
                _secondaryEnumerator = secondaryEnumerator;
            }

            /// <inheritdoc />
            public IEntityMapping Current
            {
                get { return _currentEnumerator.Current; }
            }

            /// <inheritdoc />
            [ExcludeFromCodeCoverage]
            [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "Not really used anymore.")]
            object IEnumerator.Current
            {
                get { return Current; }
            }

            /// <inheritdoc />
            public void Dispose()
            {
                _primaryEnumerator.Dispose();
                _secondaryEnumerator.Dispose();
                _currentEnumerator = null;
            }

            /// <inheritdoc />
            public bool MoveNext()
            {
                if (_currentEnumerator.MoveNext())
                {
                    return true;
                }

                if (_currentEnumerator == _secondaryEnumerator)
                {
                    return false;
                }

                _currentEnumerator = _secondaryEnumerator;
                return _currentEnumerator.MoveNext();
            }

            /// <inheritdoc />
            public void Reset()
            {
                _primaryEnumerator.Reset();
                _secondaryEnumerator.Reset();
                _currentEnumerator = _primaryEnumerator;
            }
        }
    }
}
