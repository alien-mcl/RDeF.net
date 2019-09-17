using System;
using System.Collections.Generic;
using RollerCaster;

namespace RDeF.Entities
{
    internal class LazyLoadedEntity : IEntity, IProxy
    {
        private readonly IEntityContext _context;
        private bool _isInitialized;

        internal LazyLoadedEntity(IEntityContext context, Iri iri)
        {
            _context = context;
            Iri = iri;
        }

        /// <inheritdoc />
        public Iri Iri { get; }

        /// <inheritdoc />
        public IEnumerable<Relation> UnmappedRelations
        {
            get
            {
                EnsureIntialized();
                return Relation.Object.UnmappedRelations;
            }
        }

        /// <inheritdoc />
        public IEntityContext Context
        {
            get
            {
                EnsureIntialized();
                return Relation.Object.Context;
            }
        }
        
        /// <inheritdoc />
        MulticastObject IProxy.WrappedObject
        {
            get
            {
                EnsureIntialized();
                return Relation.Object.Unwrap();
            }
        }

        /// <inheritdoc />
        Type IProxy.CurrentCastedType
        {
            get
            {
                EnsureIntialized();
                return typeof(IEntity);
            }
        }

        internal Relation Relation { get; set; }

        private void EnsureIntialized()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                var entity = _context.Load<IEntity>(Iri).Result;
                Relation.Initialize(entity);
            }
        }
    }
}
