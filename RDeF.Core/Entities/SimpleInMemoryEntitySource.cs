using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using RDeF.Collections;
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
            IEnumerable<KeyValuePair<IEntity, ISet<Statement>>> retractedStatements,
            IEnumerable<KeyValuePair<IEntity, ISet<Statement>>> addedStatements)
        {
            lock (_sync)
            {
                ProcessStatements(retractedStatements, (statements, statement) => statements.Remove(statement));
                ProcessStatements(addedStatements, (statements, statement) => statements.Add(statement));
                var toBeRemoved = Entities.Where(entity => entity.Value.Count == 0).Select(entity => entity.Key).ToList();
                foreach (var entity in toBeRemoved)
                {
                    Entities.Remove(entity);
                }
            }
        }

        /// <inheritdoc />
        public IEntity Create(Iri iri, IEntityContext entityContext)
        {
            lock (_sync)
            {
                var result = Entities.Where(entity => entity.Key.Iri == iri).Select(entity => entity.Key).FirstOrDefault();
                if (result == null)
                {
                    Entities[result = new Entity(iri, entityContext as DefaultEntityContext) { IsInitialized = true }] = new HashSet<Statement>();
                }

                return result;
            }
        }

        /// <inheritdoc />
        public TEntity Create<TEntity>(Iri iri, IEntityContext entityContext) where TEntity : IEntity
        {
            lock (_sync)
            {
                return Create(iri, entityContext).ActLike<TEntity>();
            }
        }

        /// <inheritdoc />
        public IQueryable<TEntity> AsQueryable<TEntity>() where TEntity : IEntity
        {
            return Entities.Keys.Where(entity => entity.Is<TEntity>()).Select(entity => entity.ActLike<TEntity>()).AsQueryable();
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
