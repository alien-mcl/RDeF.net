using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RDeF.Collections;
using RDeF.Mapping;
using RollerCaster;

namespace RDeF.Entities
{
    /// <summary>Provides a default implementation of the <see cref="IEntityContext" />.</summary>
    public class DefaultEntityContext : IEntityContext
    {
        private readonly AutoResetEvent _sync = new AutoResetEvent(true);
        private readonly IEntitySource _entitySource;
        private readonly IChangeDetector _changeDetector;
        private readonly IEnumerable<ILiteralConverter> _literalConverters;
        private readonly IDictionary<Iri, Entity> _entityCache;
        private readonly ICollection<Iri> _deletedEntities;
        private bool _disposed;

        internal DefaultEntityContext(
            IEntitySource entitySource,
            IMappingsRepository mappingsRepository,
            IChangeDetector changeDetector,
            IEnumerable<ILiteralConverter> literalConverters)
        {
            _entitySource = entitySource;
            Mappings = mappingsRepository;
            _changeDetector = changeDetector;
            _literalConverters = literalConverters;
            _deletedEntities = new List<Iri>();
            _entityCache = new ConcurrentDictionary<Iri, Entity>();
        }

        /// <inheritdoc />
        public event EventHandler Disposed;

        /// <inheritdoc />
        public event EventHandler<UnmappedPropertyEventArgs> UnmappedPropertyEncountered;

        /// <inheritdoc />
        public virtual IMappingsRepository Mappings { get; }

        /// <inheritdoc />
        public virtual IReadableEntitySource EntitySource { get { return _entitySource; } }

        /// <inheritdoc />
        public virtual Task<TEntity> Load<TEntity>(Iri iri) where TEntity : IEntity
        {
            return Load<TEntity>(iri, CancellationToken.None);
        }

        /// <inheritdoc />
        public virtual async Task<TEntity> Load<TEntity>(Iri iri, CancellationToken cancellationToken) where TEntity : IEntity
        {
            if (iri == null)
            {
                throw new ArgumentNullException(nameof(iri));
            }

            return (await CreateInternal(iri, false, cancellationToken)).ActLike<TEntity>();
        }

        /// <inheritdoc />
        public virtual TEntity Create<TEntity>(Iri iri) where TEntity : IEntity
        {
            if (iri == null)
            {
                throw new ArgumentNullException(nameof(iri));
            }

            return CreateInternal(iri, true, CancellationToken.None).Result.ActLike<TEntity>();
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

            try
            {
                _sync.WaitOne();
                Entity result;
                if (_entityCache.TryGetValue(iri, out result))
                {
                    _entityCache.Remove(iri);
                    _deletedEntities.Add(iri);
                }
            }
            finally
            {
                _sync.Set();
            }
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
        public virtual Task Commit()
        {
            return Commit(CancellationToken.None);
        }

        /// <inheritdoc />
        public virtual Task Commit(IEnumerable<Iri> onlyTheseResources)
        {
            return Commit(onlyTheseResources, CancellationToken.None);
        }

        /// <inheritdoc />
        public virtual Task Commit(CancellationToken cancellationToken)
        {
            return Commit(null, cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task Commit(IEnumerable<Iri> onlyTheseResources, CancellationToken cancellationToken)
        {
            var areThereAnyResourceCommitLimitations = onlyTheseResources != null && onlyTheseResources.Any();
            try
            {
                _sync.WaitOne();
                IDictionary<IEntity, ISet<Statement>> retractedStatements = new Dictionary<IEntity, ISet<Statement>>();
                IDictionary<IEntity, ISet<Statement>> addedStatements = new Dictionary<IEntity, ISet<Statement>>();
                var comittedEntities = new List<Entity>();
                foreach (var entity in _entityCache)
                {
                    if (!areThereAnyResourceCommitLimitations || onlyTheseResources.Contains(entity.Key))
                    {
                        lock (entity.Value.SynchronizationContext)
                        {
                            _changeDetector.Process(entity.Value, retractedStatements, addedStatements);
                            comittedEntities.Add(entity.Value);
                        }
                    }
                }

                await _entitySource.Commit(_deletedEntities, retractedStatements, addedStatements, cancellationToken);
                foreach (var entity in comittedEntities)
                {
                    entity.IsInitialized = true;
                }
            }
            finally
            {
                _sync.Set();
            }
        }

        /// <inheritdoc />
        public virtual void Rollback(IEnumerable<Iri> onlyTheseResources = null)
        {
            var areThereAnyResourceCommitLimitations = onlyTheseResources != null && onlyTheseResources.Any();
            try
            {
                _sync.WaitOne();
                _deletedEntities.Clear();
                foreach (var entity in _entityCache)
                {
                    if (!areThereAnyResourceCommitLimitations || onlyTheseResources.Contains(entity.Key))
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
                                entity.Value.SetPropertyInternal(propertyValue.Property, propertyValue.Value, null);
                            }

                            entity.Value.IsInitialized = true;
                        }
                    }
                }
            }
            finally
            {
                _sync.Reset();
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal virtual async Task Initialize(Entity entity, CancellationToken cancellationToken)
        {
            //// TODO: Think about currating the resulting data set against i.e. ontology to trim unnecessary proxy property values.
            var context = new EntityInitializationContext();
            await InitializeInternal(entity, await _entitySource.Load(entity.Iri, cancellationToken), context, null, cancellationToken);
        }

        internal virtual void Clear()
        {
            try
            {
                _sync.WaitOne();
                _entityCache.Clear();
            }
            finally
            {
                _sync.Set();
            }
        }

        internal virtual Task<Entity> CreateInternal(Iri iri, bool isInitialized, CancellationToken cancellationToken)
        {
            Entity result;
            if (_entityCache.TryGetValue(iri, out result))
            {
                return Task.FromResult(result);
            }

            var inMemoryEntitySource = _entitySource as IInMemoryEntitySource;
            if (inMemoryEntitySource != null)
            {
                return Task.FromResult(_entityCache[iri] = inMemoryEntitySource.Create(iri) as Entity);
            }

            return Task.FromResult(CreateInternal(new Entity(iri, this) { IsInitialized = isInitialized }));
        }

        internal virtual Entity CreateInternal(Entity entity)
        {
            return _entityCache[entity.Iri] = entity;
        }

        internal virtual async Task InitializeInternal(
            Entity entity,
            IEnumerable<Statement> statements,
            EntityInitializationContext context,
            Action<Statement> onIteration,
            CancellationToken cancellationToken)
        {
            if (onIteration == null)
            {
                onIteration = _ => { };
            }

            foreach (var statement in statements)
            {
                onIteration(statement);
                if (!statement.IsRelatedTo(entity))
                {
                    context.EntityStatements.EnsureKey(statement.Subject).Add(statement);
                    continue;
                }

                if (statement.IsTypeAssertion())
                {
                    var entityMapping = Mappings.FindEntityMappingFor(entity, statement.Object, statement.Graph);
                    if (entityMapping != null)
                    {
                        entity.CastedTypes.Add(entityMapping.Type);
                    }
                }

                var propertyMappings = Mappings.FindPropertyMappingsFor(entity, statement.Predicate, statement.Graph);
                if (!propertyMappings.Any() && UnmappedPropertyEncountered != null)
                {
                    var e = new UnmappedPropertyEventArgs(this, statement);
                    UnmappedPropertyEncountered(this, e);
                    if (e.PropertyMapping != null)
                    {
                        propertyMappings = new[] { e.PropertyMapping };
                    }
                }

                if (!propertyMappings.Any())
                {
                    entity.SetUnmappedProperty(statement, _literalConverters);
                    continue;
                }

                foreach (var propertyMapping in propertyMappings)
                {
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

                    try
                    {
                        entity.SetProperty(statement, propertyMapping, context);
                    }
                    catch (Exception error)
                    {
                        var message = String.Format(
                            "Unable to set property of '{0}' of object {1} mapped to {2} with value of {3}.",
                            propertyMapping.Name,
                            statement.Subject,
                            statement.Predicate,
                            statement.Object ?? statement.Value);
                        throw new InvalidOperationException(message, error);
                    }
                }
            }

            entity.InitializeLists(context);
            entity.IsInitialized = true;
            await InitializeChildEntities(context, cancellationToken);
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
            _sync.Dispose();
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        private async Task InitializeChildEntities(EntityInitializationContext context, CancellationToken cancellationToken)
        {
            foreach (var otherEntity in context.EntitiesCreated.Where(otherEntity => !otherEntity.IsInitialized))
            {
                IEnumerable<Statement> statements = null;
                ISet<Statement> otherStatements;
                if (!context.EntityStatements.TryGetValue(otherEntity.Iri, out otherStatements))
                {
                    statements = await _entitySource.Load(otherEntity.Iri, cancellationToken);
                }

                await InitializeInternal(otherEntity, otherStatements ?? statements, context, null, cancellationToken);
                context.EntityStatements.Remove(otherEntity.Iri);
            }
        }
    }
}
