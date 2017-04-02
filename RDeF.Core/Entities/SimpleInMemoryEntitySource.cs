using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RDeF.Collections;
using RDeF.Serialization;
using RollerCaster;

namespace RDeF.Entities
{
    /// <summary>Provides a very simple in-memory based implementation of the <see cref="IEntitySource" />.</summary>
    public sealed class SimpleInMemoryEntitySource : IInMemoryEntitySource
    {
        private readonly object _sync = new Object();

        internal SimpleInMemoryEntitySource()
        {
            Entities = new ConcurrentDictionary<IEntity, ISet<Statement>>();
        }

        internal IDictionary<IEntity, ISet<Statement>> Entities { get; }

        /// <inheritdoc />
        public IEnumerable<Statement> Load(Iri iri)
        {
            throw new NotSupportedException("In-Memory entity source doesn't support entity loading.");
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

            lock (_sync)
            {
                ProcessStatements(retractedStatements, (statements, statement) => statements.Remove(statement));
                ProcessStatements(addedStatements, (statements, statement) => statements.Add(statement));
                var toBeRemoved = Entities.Where(entity => entity.Value.Count == 0 || deletedEntities.Contains(entity.Key.Iri)).Select(entity => entity.Key).ToList();
                foreach (var entity in toBeRemoved)
                {
                    Entities.Remove(entity);
                }
            }
        }

        /// <inheritdoc />
        public IEntity Create(Iri iri, IEntityContext entityContext)
        {
            if (iri == null)
            {
                throw new ArgumentNullException(nameof(iri));
            }

            if (entityContext == null)
            {
                throw new ArgumentNullException(nameof(entityContext));
            }

            var defaultEntityContext = entityContext as DefaultEntityContext;
            if (defaultEntityContext == null)
            {
                throw new ArgumentOutOfRangeException(nameof(entityContext));
            }

            lock (_sync)
            {
                var result = Entities.Where(entity => entity.Key.Iri == iri).Select(entity => entity.Key).FirstOrDefault();
                if (result == null)
                {
                    Entities[result = new Entity(iri, defaultEntityContext) { IsInitialized = true }] = new HashSet<Statement>();
                }

                return result;
            }
        }

        /// <inheritdoc />
        public TEntity Create<TEntity>(Iri iri, IEntityContext entityContext) where TEntity : IEntity
        {
            return Create(iri, entityContext).ActLike<TEntity>();
        }

        /// <inheritdoc />
        public void Delete(Iri iri)
        {
            if (iri == null)
            {
                throw new ArgumentNullException(nameof(iri));
            }

            var existingEntity = Entities.Where(entity => entity.Key.Iri == iri).Select(entity => entity.Key).FirstOrDefault();
            if (existingEntity != null)
            {
                Entities.Remove(existingEntity);
            }
        }

        /// <inheritdoc />
        public IQueryable<TEntity> AsQueryable<TEntity>() where TEntity : IEntity
        {
            if (typeof(TEntity) == typeof(IEntity))
            {
                return (IQueryable<TEntity>)Entities.Keys.AsQueryable();
            }

            return Entities.Keys.Where(entity => entity.Is<TEntity>()).Select(entity => entity.ActLike<TEntity>()).AsQueryable();
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

            var graphs = from entity in Entities
                         from statement in entity.Value
                         group statement by statement.Graph into graph
                         select new KeyValuePair<Iri, IEnumerable<Statement>>(graph.Key, graph);
            await rdfWriter.Write(streamWriter, graphs);
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
