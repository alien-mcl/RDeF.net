using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RDeF.Collections;
using RDeF.Mapping;
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
        public Task<IEnumerable<Statement>> Load(Iri iri)
        {
            return Load(iri, CancellationToken.None);
        }

        /// <inheritdoc />
        public Task<IEnumerable<Statement>> Load(Iri iri, CancellationToken cancellationToken)
        {
            try
            {
                _sync.WaitOne();
                _sync.Reset();
                IEntity entity;
                ISet<Statement> result;
                if (iri != null && EntityMap.TryGetValue(iri, out entity) && Entities.TryGetValue(entity, out result))
                {
                    return Task.FromResult<IEnumerable<Statement>>(result);
                }
            }
            finally
            {
                _sync.Set();
            }

            return Task.FromResult<IEnumerable<Statement>>(Array.Empty<Statement>());
        }

        /// <inheritdoc />
        public Task Commit(
            IEnumerable<Iri> deletedEntities,
            IEnumerable<KeyValuePair<IEntity, ISet<Statement>>> retractedStatements,
            IEnumerable<KeyValuePair<IEntity, ISet<Statement>>> addedStatements)
        {
            return Commit(deletedEntities, retractedStatements, addedStatements, CancellationToken.None);
        }

        /// <inheritdoc />
        public Task Commit(
            IEnumerable<Iri> deletedEntities,
            IEnumerable<KeyValuePair<IEntity, ISet<Statement>>> retractedStatements,
            IEnumerable<KeyValuePair<IEntity, ISet<Statement>>> addedStatements,
            CancellationToken cancellationToken)
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

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        IEntity IInMemoryEntitySource.Create(Iri iri)
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
        TEntity IInMemoryEntitySource.Create<TEntity>(Iri iri)
        {
            return ((IInMemoryEntitySource)this).Create(iri).ActLike<TEntity>();
        }

        /// <inheritdoc />
        public Task<IEntity> Create(Iri iri)
        {
            return Create(iri, CancellationToken.None);
        }

        /// <inheritdoc />
        public Task<IEntity> Create(Iri iri, CancellationToken cancellationToken)
        {
            return Task.FromResult(((IInMemoryEntitySource)this).Create(iri));
        }

        /// <inheritdoc />
        public Task<TEntity> Create<TEntity>(Iri iri) where TEntity : IEntity
        {
            return Create<TEntity>(iri, CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<TEntity> Create<TEntity>(Iri iri, CancellationToken cancellationToken) where TEntity : IEntity
        {
            return (await Create(iri, cancellationToken)).ActLike<TEntity>();
        }

        /// <inheritdoc />
        public IQueryable<TEntity> AsQueryable<TEntity>() where TEntity : IEntity
        {
            Func<IEnumerable<IEntity>, IQueryable> selector;
            if (!StandardQueryableTypes.TryGetValue(typeof(TEntity), out selector))
            {
                var entityMappings = _entityContext().Mappings.FindEntityMappingsFor<TEntity>();
                if (entityMappings.Any())
                {
                    var types = entityMappings.SelectMany(_ => _.Classes).Select(_ => _.Term).Distinct().ToArray();
                    if (types.Length == 0)
                    {
                        selector = entities => entities
                            .Select(entity => entity.ActLike<TEntity>())
                            .AsQueryable();
                    }
                    else
                    {
                        selector = entities => entities
                            .Where(entity => types.Any(entity.Is))
                            .Select(entity => entity.ActLike<TEntity>())
                            .AsQueryable();
                    }
                }
                else
                {
                    selector = entities => entities
                        .Where(entity => entity.Is<TEntity>())
                        .Select(entity => entity.ActLike<TEntity>())
                        .AsQueryable();
                }
            }

            return (IQueryable<TEntity>)selector(Entities.Keys);
        }

        /// <inheritdoc />
        public Task Write(StreamWriter streamWriter, IRdfWriter rdfWriter)
        {
            return Write(streamWriter, rdfWriter, CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task Write(StreamWriter streamWriter, IRdfWriter rdfWriter, CancellationToken cancellationToken)
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
                             select new Graph(graph.Key, graph);
                await rdfWriter.Write(streamWriter, graphs, cancellationToken);
            }
            finally
            {
                _sync.Set();
            }
        }

        /// <inheritdoc />
        public Task Read(StreamReader streamReader, IRdfReader rdfReader)
        {
            return Read(streamReader, rdfReader, null);
        }

        /// <inheritdoc />
        public Task Read(StreamReader streamReader, IRdfReader rdfReader, Uri baseUri)
        {
            return Read(streamReader, rdfReader, baseUri, CancellationToken.None);
        }

        /// <inheritdoc />
        public Task Read(StreamReader streamReader, IRdfReader rdfReader, CancellationToken cancellationToken)
        {
            return Read(streamReader, rdfReader, null, cancellationToken);
        }

        /// <inheritdoc />
        public async Task Read(StreamReader streamReader, IRdfReader rdfReader, Uri baseUri, CancellationToken cancellationToken)
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
                foreach (var graph in await rdfReader.Read(streamReader, baseUri, cancellationToken))
                {
                    foreach (var statement in graph.Statements)
                    {
                        statement.EnsureCache(subjects);
                        additionalStatements?.Invoke(subjects, statement);
                    }
                }

                foreach (var subject in subjects)
                {
                    var entity = await entityContext.CreateInternal(subject.Key, true, cancellationToken);
                    await entityContext.InitializeInternal(
                        entity,
                        subject.Value,
                        new EntityInitializationContext(),
                        statement => Entities[EntityMap[entity.Iri] = entity].Add(statement),
                        cancellationToken);
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
