using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RDeF.Collections;
using RDeF.Serialization;
using RollerCaster;

namespace RDeF.Entities
{
    /// <summary>Provides a very simple in-memory based implementation of the <see cref="IEntitySource" />.</summary>
    public sealed class SimpleInMemoryEntitySource : IInMemoryEntitySource, IDisposable
    {
        private static readonly IDictionary<Type, Func<IEnumerable<IEntity>, IQueryable>> StandardQueryableTypes =
            new Dictionary<Type, Func<IEnumerable<IEntity>, IQueryable>>()
            {
                { typeof(IEntity), entities => entities.AsQueryable() },
                { typeof(ITypedEntity), entities => entities.Select(_ => _.ActLike<ITypedEntity>()).AsQueryable() }
            };

        private readonly ManualResetEvent _sync = new ManualResetEvent(true);
        private readonly Func<DefaultEntityContext> _entityContext;
        private bool _loading;

        internal SimpleInMemoryEntitySource(Func<DefaultEntityContext> entityContext)
        {
            _entityContext = entityContext;
            Entities = new Dictionary<IEntity, ISet<Statement>>();
            EntityMap = new Dictionary<Iri, IEntity>();
        }

        /// <inheritdoc />
        public event EventHandler<StatementEventArgs> StatementAsserted;

        /// <inheritdoc />
        public IEnumerable<Statement> Statements
        {
            get
            {
                return from entity in Entities
                       from statement in entity.Value
                       select statement;
            }
        }

        internal IDictionary<IEntity, ISet<Statement>> Entities { get; }

        internal IDictionary<Iri, IEntity> EntityMap { get; }

        /// <inheritdoc />
        public IEnumerable<Statement> Load(Iri iri)
        {
            try
            {
                _sync.WaitOne();
                _sync.Reset();
                IEntity entity;
                ISet<Statement> result;
                if (iri != null && EntityMap.TryGetValue(iri, out entity) && Entities.TryGetValue(entity, out result))
                {
                    return result;
                }
            }
            finally
            {
                _sync.Set();
            }

            return Array.Empty<Statement>();
        }

        /// <inheritdoc />
        public void Commit(
            IEnumerable<Iri> deletedEntities,
            IEnumerable<KeyValuePair<IEntity, ISet<Statement>>> retractedStatements,
            IEnumerable<KeyValuePair<IEntity, ISet<Statement>>> addedStatements)
        {
            if (deletedEntities == null)
            {
                throw new ArgumentNullException(nameof(deletedEntities));
            }

            if (retractedStatements == null)
            {
                throw new ArgumentNullException(nameof(retractedStatements));
            }

            if (addedStatements == null)
            {
                throw new ArgumentNullException(nameof(addedStatements));
            }

            try
            {
                _sync.WaitOne();
                _sync.Reset();
                ProcessStatements(retractedStatements, (statements, statement) => statements.Remove(statement));
                ProcessStatements(addedStatements, (statements, statement) => statements.Add(statement));
                var toBeRemoved = Entities.Where(entity => deletedEntities.Contains(entity.Key.Iri)).Select(entity => entity.Key).ToList();
                foreach (var entity in toBeRemoved)
                {
                    DeleteInternal(entity);
                }
            }
            finally
            {
                _sync.Set();
            }
        }

        /// <inheritdoc />
        public IEntity Create(Iri iri)
        {
            if (iri == null)
            {
                throw new ArgumentNullException(nameof(iri));
            }

            try
            {
                if (!_loading)
                {
                    _sync.WaitOne();
                    _sync.Reset();
                }

                IEntity result;
                if (!EntityMap.TryGetValue(iri, out result))
                {
                    Entities[result = new Entity(iri, _entityContext()) { IsInitialized = true }] = new HashSet<Statement>();
                    EntityMap[iri] = result;
                }

                return result;
            }
            finally
            {
                if (!_loading)
                {
                    _sync.Set();
                }
            }
        }

        /// <inheritdoc />
        public TEntity Create<TEntity>(Iri iri) where TEntity : IEntity
        {
            return Create(iri).ActLike<TEntity>();
        }

        /// <inheritdoc />
        public void Delete(Iri iri)
        {
            if (iri == null)
            {
                throw new ArgumentNullException(nameof(iri));
            }

            try
            {
                _sync.WaitOne();
                _sync.Reset();
                IEntity existingEntity;
                if (EntityMap.TryGetValue(iri, out existingEntity))
                {
                    DeleteInternal(existingEntity);
                }
            }
            finally
            {
                _sync.Set();
            }
        }

        /// <inheritdoc />
        public IQueryable<TEntity> AsQueryable<TEntity>() where TEntity : IEntity
        {
            Func<IEnumerable<IEntity>, IQueryable> selector;
            if (!StandardQueryableTypes.TryGetValue(typeof(TEntity), out selector))
            {
                selector = entities => entities.Where(entity => entity.Is<TEntity>()).Select(entity => entity.ActLike<TEntity>()).AsQueryable();
            }

            return (IQueryable<TEntity>)selector(Entities.Keys);
        }

        /// <inheritdoc />
        public async Task Write(StreamWriter streamWriter, IRdfWriter rdfWriter)
        {
            if (streamWriter == null)
            {
                throw new ArgumentNullException(nameof(streamWriter));
            }

            if (rdfWriter == null)
            {
                throw new ArgumentNullException(nameof(rdfWriter));
            }

            try
            {
                _sync.WaitOne();
                _sync.Reset();
                var graphs = from entity in Entities
                             from statement in entity.Value
                             group statement by statement.Graph
                             into graph
                             select new KeyValuePair<Iri, IEnumerable<Statement>>(graph.Key, graph);
                await rdfWriter.Write(streamWriter, graphs);
            }
            finally
            {
                _sync.Set();
            }
        }

        /// <inheritdoc />
        public async Task Read(StreamReader streamReader, IRdfReader rdfReader)
        {
            if (streamReader == null)
            {
                throw new ArgumentNullException(nameof(streamReader));
            }

            if (rdfReader == null)
            {
                throw new ArgumentNullException(nameof(rdfReader));
            }

            try
            {
                _sync.WaitOne();
                _sync.Reset();
                _loading = true;
                var entityContext = _entityContext();
                entityContext.Clear();
                Entities.Clear();
                EntityMap.Clear();
                var subjects = new Dictionary<Iri, ISet<Statement>>();
                Action<IDictionary<Iri, ISet<Statement>>, Statement> additionalStatements =
                    StatementAsserted != null ? AssertAdditionalStatements : (Action<IDictionary<Iri, ISet<Statement>>, Statement>)null;
                foreach (var graph in await rdfReader.Read(streamReader))
                {
                    foreach (var statement in graph.Value)
                    {
                        statement.EnsureCache(subjects);
                        additionalStatements?.Invoke(subjects, statement);
                    }
                }

                foreach (var subject in subjects)
                {
                    var entity = entityContext.CreateInternal(subject.Key);
                    entityContext.InitializeInternal(
                        entity,
                        subject.Value,
                        new EntityInitializationContext(),
                        statement => Entities[EntityMap[entity.Iri] = entity].Add(statement));
                }
            }
            finally
            {
                _loading = false;
                _sync.Set();
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _sync.Dispose();
        }

        private void DeleteInternal(IEntity entity)
        {
            Entities.Remove(entity);
            EntityMap.Remove(entity.Iri);
        }

        private void AssertAdditionalStatements(IDictionary<Iri, ISet<Statement>> subjects, Statement statement)
        {
            var e = new StatementEventArgs(statement);
            StatementAsserted.Invoke(this, e);
            foreach (var additionalStatementToAssert in e.AdditionalStatementsToAssert)
            {
                additionalStatementToAssert.EnsureCache(subjects);
            }
        }

        private void ProcessStatements(IEnumerable<KeyValuePair<IEntity, ISet<Statement>>> entityStatements, Action<ISet<Statement>, Statement> action)
        {
            IEntity lastEntity = null;
            ISet<Statement> lastEntityStatements = null;
            foreach (var retractedEntityStatements in entityStatements)
            {
                if (!Equals(lastEntity, retractedEntityStatements.Key))
                {
                    lastEntityStatements = Entities.EnsureKey(lastEntity = retractedEntityStatements.Key);
                }

                foreach (var retractedStatement in retractedEntityStatements.Value)
                {
                    action(lastEntityStatements, retractedStatement);
                }
            }
        }
    }
}
