using System;
using System.Collections.Generic;
using System.Diagnostics;
using RollerCaster;

namespace RDeF.Entities
{
    /// <summary>Provides a wrapper class over RDF data entity.</summary>
    [DebuggerDisplay("<{Iri,nq}>")]
    internal sealed class Entity : MulticastObject, IEntity
    {
        private readonly DefaultEntityContext _context;
        private bool _isInitialized;

        /// <summary>Initializes a new instance of the <see cref="Entity"/> class.</summary>
        /// <param name="id">The identifier.</param>
        /// <param name="context">The entity context.</param>
        internal Entity(Iri id, DefaultEntityContext context)
        {
            Iri = id;
            _isInitialized = false;
            _context = context;
            OriginalValues = new HashSet<MulticastPropertyValue>();
        }

        /// <inheritdoc />
        public Iri Iri { get; }

        /// <inheritdoc />
        public IEntityContext Context { get { return _context; } }

        internal object SynchronizationContext { get { return Sync; } }

        internal IEnumerable<Type> OriginalTypes { get; set; }

        internal ISet<MulticastPropertyValue> OriginalValues { get; set; }

        internal bool IsChanged { get; set; }

        internal bool IsInitialized
        {
            get
            {
                return _isInitialized;
            }

            set
            {
                if (_isInitialized = value)
                {
                    lock (Sync)
                    {
                        OriginalTypes = new HashSet<Type>(CastedTypes);
                        OriginalValues = new HashSet<MulticastPropertyValue>(PropertyValues);
                        IsChanged = false;
                    }
                }
            }
        }

        /// <inheritdoc />
        public override object GetProperty(Type objectType, string propertyName)
        {
            EnsureInitialized();
            IsChanged = true;
            return GetPropertyInternal(objectType, propertyName);
        }

        /// <inheritdoc />
        public override void SetProperty(Type objectType, string propertyName, object value)
        {
            EnsureInitialized();
            IsChanged = true;
            SetPropertyInternal(objectType, propertyName, value);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            Entity anotherEntity = obj as Entity;
            return (ReferenceEquals(this, obj)) || ((obj != null) && (Equals(Iri, anotherEntity.Iri)));
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Iri.GetHashCode();
        }

        internal object GetPropertyInternal(Type objectType, string propertyName)
        {
            return base.GetProperty(objectType, propertyName);
        }

        internal void SetPropertyInternal(Type objectType, string propertyName, object value)
        {
            base.SetProperty(objectType, propertyName, value);
        }

        private void EnsureInitialized()
        {
            lock (Sync)
            {
                if (IsInitialized)
                {
                    return;
                }

                IsInitialized = true;
                _context.Initialize(this);
            }
        }
    }
}
