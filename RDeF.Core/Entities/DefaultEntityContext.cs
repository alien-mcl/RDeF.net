using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using RDeF.Collections;
using RDeF.Mapping;
using RollerCaster;

namespace RDeF.Entities
{
    /// <summary>Provides a default implementation of the <see cref="IEntityContext" />.</summary>
    public class DefaultEntityContext : IEntityContext
    {
        private readonly object _sync = new Object();
        private readonly IEntitySource _entitySource;
        private readonly IChangeDetector _changeDetector;
        private readonly IDictionary<Iri, Entity> _entityCache;
        private readonly ICollection<Iri> _deletedEntities;
        private bool _disposed;

        internal DefaultEntityContext(IEntitySource entitySource, IMappingsRepository mappingsRepository, IChangeDetector changeDetector)
        {
            _entitySource = entitySource;
            Mappings = mappingsRepository;
            _changeDetector = changeDetector;
            _deletedEntities = new List<Iri>();
            _entityCache = new ConcurrentDictionary<Iri, Entity>();
        }

        /// <inheritdoc />
        public event EventHandler Disposed;

        /// <inheritdoc />
        public virtual IMappingsRepository Mappings { get; }

        /// <inheritdoc />
        public virtual IReadableEntitySource EntitySource { get { return _entitySource; } }

        /// <inheritdoc />
        public virtual TEntity Load<TEntity>(Iri iri) where TEntity : IEntity
        {
            if (iri == null)
            {
                throw new ArgumentNullException(nameof(iri));
            }

            return CreateInternal<TEntity>(iri, false);
        }

        /// <inheritdoc />
        public virtual TEntity Create<TEntity>(Iri iri) where TEntity : IEntity
        {
            if (iri == null)
            {
                throw new ArgumentNullException(nameof(iri));
            }

            return CreateInternal<TEntity>(iri);
        }

        /// <inheritdoc />
        public virtual void Delete(Iri iri)
        {
            if (iri == null)
            {
                throw new ArgumentNullException(nameof(iri));
            }

            if (iri.IsBlank)
            {
                throw new ArgumentOutOfRangeException(nameof(iri));
            }

            lock (_sync)
            {
                Entity result;
                if (_entityCache.TryGetValue(iri, out result))
                {
                    _entityCache.Remove(iri);
                    _deletedEntities.Add(iri);
                }
            }

            (_entitySource as IInMemoryEntitySource)?.Delete(iri);
        }

        /// <inheritdoc />
        public virtual TEntity Copy<TEntity>(TEntity entity, Iri newIri = null) where TEntity : IEntity
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (entity.Context == this)
            {
                return entity;
            }

            var proxy = entity.Unwrap() as Entity;
            if (proxy == null)
            {
                throw new ArgumentOutOfRangeException(nameof(entity));
            }

            lock (proxy)
            {
                proxy.EntityContextOverride = this;
                proxy.IriOverride = newIri;
                var result = (Entity)proxy.Clone(true);
                proxy.EntityContextOverride = null;
                proxy.IriOverride = null;
                result.IsInitialized = true;
                return result.ActLike<TEntity>();
            }
        }

        /// <inheritdoc />
        public virtual IQueryable<TEntity> AsQueryable<TEntity>() where TEntity : IEntity
        {
            var inMemoryEntitySource = _entitySource as IInMemoryEntitySource;
            if (inMemoryEntitySource != null)
            {
                return inMemoryEntitySource.AsQueryable<TEntity>();
            }

            throw new NotSupportedException("Persistent RDF entity sources are not yet supported.");
        }

        /// <inheritdoc />
        public virtual void Commit()
        {
            lock (_sync)
            {
                IDictionary<IEntity, ISet<Statement>> retractedStatements = new Dictionary<IEntity, ISet<Statement>>();
                IDictionary<IEntity, ISet<Statement>> addedStatements = new Dictionary<IEntity, ISet<Statement>>();
                foreach (var entity in _entityCache)
                {
                    lock (entity.Value.SynchronizationContext)
                    {
                        _changeDetector.Process(entity.Value, retractedStatements, addedStatements);
                    }
                }

                _entitySource.Commit(_deletedEntities, retractedStatements, addedStatements);
                foreach (var entity in _entityCache)
                {
                    entity.Value.IsInitialized = true;
                }
            }
        }

        /// <inheritdoc />
        public virtual void Rollback()
        {
            lock (_sync)
            {
                _deletedEntities.Clear();
                foreach (var entity in _entityCache)
                {
                    lock (entity.Value.SynchronizationContext)
                    {
                        if (!entity.Value.IsChanged)
                        {
                            continue;
                        }

                        var valuesToSet = new List<MulticastPropertyValue>();
                        foreach (var currentPropertyValue in entity.Value.PropertyValues)
                        {
                            MulticastPropertyValue valueToSet = null;
                            var originalValue = entity.Value.OriginalValues.FindMatching(currentPropertyValue);
                            if (originalValue != null)
                            {
                                entity.Value.OriginalValues.Remove(originalValue);
                                if (originalValue.Value != null)
                                {
                                    valueToSet = originalValue;
                                }
                            }

                            if (valueToSet == null)
                            {
                                valueToSet = new MulticastPropertyValue(currentPropertyValue.CastedType, currentPropertyValue.Property, null);
                            }

                            valuesToSet.Add(valueToSet);
                        }

                        foreach (var propertyValue in valuesToSet.Concat(entity.Value.OriginalValues))
                        {
                            entity.Value.SetPropertyInternal(propertyValue.CastedType, propertyValue.Property.Name, propertyValue.Value);
                        }

                        entity.Value.IsInitialized = true;
                    }
                }
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal virtual void Initialize(Entity entity)
        {
            //// TODO: Think about currating the resulting data set against i.e. ontology to trim unnecessary proxy property values.
            var context = new EntityInitializationContext();
            InitializeInternal(entity, _entitySource.Load(entity.Iri), context);
        }

        internal Entity CreateInternal(Iri iri, bool isInitialized = true)
        {
            Entity result;
            if (_entityCache.TryGetValue(iri, out result))
            {
                return result;
            }

            var inMemoryEntitySource = _entitySource as IInMemoryEntitySource;
            if (inMemoryEntitySource != null)
            {
                return _entityCache[iri] = inMemoryEntitySource.Create(iri, this) as Entity;
            }

            return _entityCache[iri] = new Entity(iri, this) { IsInitialized = isInitialized };
        }

        /// <summary>Performs an actual disposal.</summary>
        /// <param name="disposing">Value indicating whether the disposal actually happens.</param>
        protected virtual void Dispose(bool disposing)
        {
            if ((_disposed) || (!disposing))
            {
                return;
            }

            _disposed = true;
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        private TEntity CreateInternal<TEntity>(Iri iri, bool isInitialized = true) where TEntity : IEntity
        {
            return CreateInternal(iri, isInitialized).ActLike<TEntity>();
        }

        private void InitializeInternal(Entity entity, IEnumerable<Statement> statements, EntityInitializationContext context)
        {
            foreach (var statement in statements)
            {
                if (!statement.IsRelatedTo(entity))
                {
                    context.EntityStatements.EnsureKey(statement.Subject).Add(statement);
                    continue;
                }

                if (statement.IsTypeAssertion())
                {
                    entity.CastedTypes.Add(Mappings.FindEntityMappingFor(statement.Object).Type);
                    continue;
                }

                var propertyMapping = Mappings.FindPropertyMappingFor(statement.Predicate);
                if (!statement.Matches(propertyMapping.Graph))
                {
                    continue;
                }

                //// TODO: Develop a dictionary statements handling.
                var collectionMapping = propertyMapping as ICollectionMapping;
                if (collectionMapping?.StoreAs == CollectionStorageModel.LinkedList)
                {
                    context.LinkedLists.EnsureKey(entity.Iri)[statement.Object] = collectionMapping;
                    continue;
                }

                entity.SetProperty(statement, propertyMapping, context);
            }

            entity.InitializeLists(context);
            entity.IsInitialized = true;
            InitializeChildEntities(context);
        }

        private void InitializeChildEntities(EntityInitializationContext context)
        {
            foreach (var otherEntity in context.EntitiesCreated.Where(otherEntity => !otherEntity.IsInitialized))
            {
                ISet<Statement> otherStatements;
                if (!context.EntityStatements.TryGetValue(otherEntity.Iri, out otherStatements))
                {
                    continue;
                }

                InitializeInternal(otherEntity, otherStatements, context);
                context.EntityStatements.Remove(otherEntity.Iri);
            }
        }
    }
}
