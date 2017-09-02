﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Reflection;
using RollerCaster;
using RollerCaster.Collections;

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
                        OriginalValues = new HashSet<MulticastPropertyValue>();
                        foreach (var propertyValue in PropertyValues)
                        {
                            OriginalValues.Add(propertyValue);
                            var observableCollection = propertyValue.Value as INotifyCollectionChanged;
                            if (observableCollection == null)
                            {
                                continue;
                            }

                            //// TODO: Check if that won't add another handler after comitting values.
                            observableCollection.CollectionChanged += (sender, e) => OnCollectionChanged(sender, e, propertyValue);
                        }

                        IsChanged = false;
                    }
                }
            }
        }

        internal DefaultEntityContext EntityContextOverride { get; set; }

        internal Iri IriOverride { get; set; }

        /// <inheritdoc />
        public override object GetProperty(PropertyInfo propertyInfo)
        {
            EnsureInitialized();
            IsChanged = true;
            return base.GetProperty(propertyInfo);
        }

        /// <inheritdoc />
        public override void SetProperty(PropertyInfo propertyInfo, object value)
        {
            EnsureInitialized();
            IsChanged = true;
            SetPropertyInternal(propertyInfo, value);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            Entity anotherEntity = obj as Entity;
            if ((anotherEntity != null) && (Equals(Iri, anotherEntity.Iri)))
            {
                return true;
            }

            MulticastObject entity;
            if ((obj.TryUnwrap(out entity)) && ((anotherEntity = entity as Entity) != null))
            {
                return (ReferenceEquals(this, anotherEntity)) || (Equals(Iri, anotherEntity.Iri));
            }

            IEntity someEntity = obj as IEntity;
            if ((someEntity != null) && (Equals(Iri, someEntity.Iri)))
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Iri.GetHashCode();
        }

        internal void SetPropertyInternal(PropertyInfo propertyInfo, object value)
        {
            base.SetProperty(propertyInfo, value);
        }

        /// <inheritdoc />
        protected override MulticastObject CreateChildInstance()
        {
            var targetContext = EntityContextOverride ?? _context;
            return targetContext.CreateInternal(new Entity(IriOverride ?? Iri, targetContext));
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

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e, MulticastPropertyValue propertyValue)
        {
            var originalValue = (IList)typeof(ObservableList<>).MakeGenericType(propertyValue.Value.GetType().GetGenericArguments()[0])
                .GetConstructor(Type.EmptyTypes).Invoke(null);
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var removed in e.OldItems)
                {
                    originalValue.Add(removed);
                }
            }

            foreach (var item in (IEnumerable)propertyValue.Value)
            {
                if (((e.Action == NotifyCollectionChangedAction.Remove) && (e.OldItems.Contains(item))) ||
                    (e.Action == NotifyCollectionChangedAction.Add))
                {
                    continue;
                }

                originalValue.Add(item);
            }

            OriginalValues.Remove(propertyValue);
            OriginalValues.Add(new MulticastPropertyValue(propertyValue.CastedType, propertyValue.Property, originalValue));
            ((IObservableCollection)sender).ClearCollectionChanged();
        }
    }
}
