using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using RDeF.Entities;
using RDeF.Mapping.Explicit;

namespace RDeF.Mapping
{
    /// <summary>Provides a default implementation of the <see cref="IMappingsRepository" /> that is aware of <see cref="IEntity" /> owned mappings.</summary>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "This is not supposed to be a collection - IEnumerable implementation is only for convinience of use.")]
    public class EntityAwareMappingsRepository : IMappingsRepository
    {
        private readonly IMappingsRepository _mappingsRepository;
        private readonly IExplicitMappings _explicitMappings;
        private readonly Iri _owningEntity;

        internal EntityAwareMappingsRepository(IMappingsRepository mappingsRepository, IExplicitMappings explicitMappings, Iri owningEntity)
        {
            _mappingsRepository = mappingsRepository;
            _explicitMappings = explicitMappings;
            _owningEntity = owningEntity;
        }

        /// <inheritdoc />
        public IEntityMapping FindEntityMappingFor(Iri @class, Iri graph = null)
        {
            return _mappingsRepository.FindEntityMappingFor(@class, graph);
        }

        /// <inheritdoc />
        public IEntityMapping FindEntityMappingFor(Type type)
        {
            return _explicitMappings.FindEntityMappingFor(type, _owningEntity) ?? _mappingsRepository.FindEntityMappingFor(type);
        }

        /// <inheritdoc />
        public IEntityMapping FindEntityMappingFor<TEntity>() where TEntity : IEntity
        {
            return FindEntityMappingFor(typeof(TEntity));
        }

        /// <inheritdoc />
        public IPropertyMapping FindPropertyMappingFor(Iri predicate, Iri graph = null)
        {
            return _mappingsRepository.FindPropertyMappingFor(predicate, graph);
        }

        /// <inheritdoc />
        public IPropertyMapping FindPropertyMappingFor(PropertyInfo property)
        {
            return _explicitMappings.FindPropertyMappingFor(property, _owningEntity) ?? _mappingsRepository.FindPropertyMappingFor(property);
        }

        /// <inheritdoc />
        public IEnumerator<IEntityMapping> GetEnumerator()
        {
            return new JoinEnumerator(_explicitMappings.GetEnumerator(_owningEntity), _mappingsRepository.GetEnumerator());
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "Not really used anymore.")]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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
