using System;
using System.Collections.Generic;
using System.Linq;
using RollerCaster;

namespace RDeF.Entities
{
    /// <summary>Provides a very simple in-memory based implementation of the <see cref="IEntitySource" />.</summary>
    public sealed class SimpleInMemoryEntitySource : IInMemoryEntitySource
    {
        private readonly object _sync = new Object();
        private readonly ISet<IEntity> _entities = new HashSet<IEntity>();

        /// <inheritdoc />
        public IEnumerable<Statement> Load(Iri iri)
        {
            return new Statement[0];
        }

        /// <inheritdoc />
        public void Commit(
            IEnumerable<KeyValuePair<IEntity, ISet<Statement>>> retractedStatements,
            IEnumerable<KeyValuePair<IEntity, ISet<Statement>>> addedStatements)
        {
            lock (_sync)
            {
                foreach (var entityStatements in retractedStatements.Concat(addedStatements))
                {
                    _entities.Add(entityStatements.Key);
                }
            }
        }

        /// <inheritdoc />
        public IEntity Create(Iri iri, IEntityContext entityContext)
        {
            lock (_sync)
            {
                var result = _entities.FirstOrDefault(entity => entity.Iri == iri);
                if (result == null)
                {
                    _entities.Add(result = new Entity(iri, entityContext as DefaultEntityContext));
                }

                return result;
            }
        }

        /// <inheritdoc />
        public TEntity Create<TEntity>(Iri iri, IEntityContext entityContext) where TEntity : IEntity
        {
            lock (_sync)
            {
                var result = _entities.FirstOrDefault(entity => entity.Iri == iri);
                if (result == null)
                {
                    _entities.Add(result = new Entity(iri, entityContext as DefaultEntityContext));
                }

                return result.ActLike<TEntity>();
            }
        }

        /// <inheritdoc />
        public IQueryable<TEntity> AsQueryable<TEntity>() where TEntity : IEntity
        {
            return _entities.AsQueryable().OfType<TEntity>();
        }
    }
}
