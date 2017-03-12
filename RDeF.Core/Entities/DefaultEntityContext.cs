using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using RDeF.Collections;
using RDeF.Mapping;
using RDeF.Vocabularies;
using RollerCaster;
using RollerCaster.Reflection;

namespace RDeF.Entities
{
    /// <summary>Provides a default implementation of the <see cref="IEntityContext" />.</summary>
    public class DefaultEntityContext : IEntityContext
    {
        private readonly object _sync = new Object();
        private readonly IEntitySource _entitySource;
        private readonly IChangeDetector _changeDetector;
        private readonly IDictionary<Iri, Entity> _entityCache;

        internal DefaultEntityContext(IEntitySource entitySource, IMappingsRepository mappingsRepository, IChangeDetector changeDetector)
        {
            _entitySource = entitySource;
            MappingsRepository = mappingsRepository;
            _changeDetector = changeDetector;
            _entityCache = new ConcurrentDictionary<Iri, Entity>();
        }

        /// <inheritdoc />
        public IMappingsRepository MappingsRepository { get; }

        /// <inheritdoc />
        public TEntity Load<TEntity>(Iri iri) where TEntity : IEntity
        {
            if (iri == null)
            {
                throw new ArgumentNullException(nameof(iri));
            }

            return CreateInternal<TEntity>(iri, false);
        }

        /// <inheritdoc />
        public TEntity Create<TEntity>(Iri iri) where TEntity : IEntity
        {
            if (iri == null)
            {
                throw new ArgumentNullException(nameof(iri));
            }

            return CreateInternal<TEntity>(iri);
        }

        /// <inheritdoc />
        public IQueryable<TEntity> AsQueryable<TEntity>() where TEntity : IEntity
        {
            var inMemoryEntitySource = _entitySource as IInMemoryEntitySource;
            if (inMemoryEntitySource != null)
            {
                return inMemoryEntitySource.AsQueryable<TEntity>();
            }

            throw new NotSupportedException("Persistent RDF entity sources are not yet supported.");
        }

        /// <inheritdoc />
        public void Commit()
        {
            lock (_sync)
            {
                IDictionary<IEntity, ISet<Statement>> retractedStatements = new Dictionary<IEntity, ISet<Statement>>();
                IDictionary<IEntity, ISet<Statement>> addedStatements = new Dictionary<IEntity, ISet<Statement>>();
                foreach (var entity in _entityCache)
                {
                    lock (entity.Value.SynchronizationContext)
                    {
                        _changeDetector.Process(entity.Value, ref retractedStatements, ref addedStatements);
                    }
                }

                _entitySource.Commit(retractedStatements, addedStatements);
            }
        }

        /// <inheritdoc />
        public void Rollback()
        {
            lock (_sync)
            {
                foreach (var entity in _entityCache)
                {
                    lock (entity.Value.SynchronizationContext)
                    {
                        if (entity.Value.IsChanged)
                        {
                            foreach (var propertyValue in entity.Value.OriginalValues)
                            {
                                entity.Value.SetPropertyInternal(propertyValue.CastedType, propertyValue.Property.Name, propertyValue.Value);
                            }

                            entity.Value.IsInitialized = true;
                        }
                    }
                }
            }
        }

        internal virtual void Initialize(Entity entity)
        {
            //// TODO: Think about currating the resulting data set against i.e. ontology to trim unnecessary proxy property values.
            InitializeInternal(entity, _entitySource.Load(entity.Iri));
        }

        internal Entity CreateInternal(Iri id, bool isInitialized = true)
        {
            Entity result;
            if (_entityCache.TryGetValue(id, out result))
            {
                return result;
            }

            var inMemoryEntitySource = _entitySource as IInMemoryEntitySource;
            if (inMemoryEntitySource != null)
            {
                return inMemoryEntitySource.Create(id, this) as Entity;
            }

            _entityCache[id] = result = new Entity(id, this) { IsInitialized = isInitialized };
            return result;
        }

        private static bool HandleLinkedListValue(Statement statement, Dictionary<Iri, ISet<Statement>> otherEntityStatements)
        {
            if ((statement.Predicate == rdf.first) || (statement.Predicate == rdf.last))
            {
                otherEntityStatements.EnsureKey(statement.Subject).Add(statement);
                return true;
            }

            return false;
        }

        private static void BuildList(Entity entity, Iri head, Dictionary<Iri, ISet<Statement>> otherEntityStatements, ICollectionMapping collectionMapping, Dictionary<Entity, Entity> entities)
        {
            Iri previousHead = null;
            while (true)
            {
                ISet<Statement> statements;
                if ((previousHead == head) || (!otherEntityStatements.TryGetValue(head, out statements)))
                {
                    break;
                }

                previousHead = head;
                foreach (var statement in statements)
                {
                    if (statement.Predicate == rdf.first)
                    {
                        var value = collectionMapping.ValueConverter.ConvertFrom(statement);
                        entity.SetPropertyInternal(collectionMapping.EntityMapping.Type, collectionMapping.Name, value);
                        var otherEntity = value as IEntity;
                        if (otherEntity != null)
                        {
                            var unwrappedEntity = (Entity)otherEntity.Unwrap();
                            entities[unwrappedEntity] = unwrappedEntity;
                        }
                    }
                    else if (statement.Predicate == rdf.last)
                    {
                        head = statement.Object;
                    }
                }
            }
        }

        private void InitializeInternal(Entity entity, IEnumerable<Statement> statements, Dictionary<Iri, ISet<Statement>> otherEntityStatements = null)
        {
            otherEntityStatements = otherEntityStatements ?? new Dictionary<Iri, ISet<Statement>>();
            var lists = new Dictionary<Iri, ISet<Statement>>();
            var entities = new Dictionary<Entity, Entity>();
            foreach (var statement in statements)
            {
                if (statement.Predicate == rdfs.type)
                {
                    entity.CastedTypes.Add(MappingsRepository.FindEntityMappingFor(statement.Object).Type);
                    continue;
                }

                HandlePropertyValue(entity, statement, otherEntityStatements, lists, entities);
            }

            BuildLists(entity, otherEntityStatements, lists, entities);
            entity.IsInitialized = true;
            foreach (var otherEntity in entities)
            {
                ISet<Statement> otherStatements;
                if (otherEntityStatements.TryGetValue(otherEntity.Value.Iri, out otherStatements))
                {
                    InitializeInternal(otherEntity.Key, otherStatements, otherEntityStatements);
                }
                else
                {
                    Initialize(otherEntity.Key);
                }
            }
        }

        private void BuildLists(Entity entity, Dictionary<Iri, ISet<Statement>> otherEntityStatements, Dictionary<Iri, ISet<Statement>> lists, Dictionary<Entity, Entity> entities)
        {
            foreach (var list in lists)
            {
                if (list.Key != entity.Iri)
                {
                    BuildLists(CreateInternal(list.Key), otherEntityStatements, lists, entities);
                    continue;
                }

                foreach (var head in list.Value)
                {
                    var collectionMapping = MappingsRepository.FindPropertyMappingFor(head.Predicate) as ICollectionMapping;
                    BuildList(entity, head.Object, otherEntityStatements, collectionMapping, entities);
                }
            }
        }

        private void HandlePropertyValue(Entity entity, Statement statement, Dictionary<Iri, ISet<Statement>> otherEntityStatements, Dictionary<Iri, ISet<Statement>> lists, Dictionary<Entity, Entity> entities)
        {
            if (HandleLinkedListValue(statement, otherEntityStatements))
            {
                return;
            }

            if (statement.Subject != entity.Iri)
            {
                otherEntityStatements.EnsureKey(statement.Subject).Add(statement);
                return;
            }

            var propertyMapping = MappingsRepository.FindPropertyMappingFor(statement.Predicate);
            if ((propertyMapping.Graph != null) && (propertyMapping.Graph != statement.Graph))
            {
                return;
            }

            var collectionMapping = propertyMapping as ICollectionMapping;
            if ((collectionMapping != null) && (collectionMapping.StoreAs == CollectionStorageModel.LinkedList))
            {
                lists.EnsureKey(entity.Iri).Add(statement);
                return;
            }

            var value = propertyMapping.ValueConverter.ConvertFrom(statement);
            var otherEntity = value as IEntity;
            if (otherEntity != null)
            {
                var unwrappedEntity = (Entity)otherEntity.Unwrap();
                entities[unwrappedEntity] = unwrappedEntity;
            }

            if (value == null)
            {
                entity.SetPropertyInternal(propertyMapping.EntityMapping.Type, propertyMapping.Name, null);
                return;
            }

            if (value.GetType().IsAnEnumerableType())
            {
                if (value.GetType().IsADictionary())
                {
                    //// TODO: Develop a dictionary statements handling.
                }
                else
                {
                    ((IList)entity.GetPropertyInternal(propertyMapping.EntityMapping.Type, propertyMapping.Name)).Add(value);
                }

                return;
            }

            entity.SetPropertyInternal(propertyMapping.EntityMapping.Type, propertyMapping.Name, value);
        }

        private TEntity CreateInternal<TEntity>(Iri id, bool isInitialized = true) where TEntity : IEntity
        {
            return CreateInternal(id, isInitialized).ActLike<TEntity>();
        }
    }
}
