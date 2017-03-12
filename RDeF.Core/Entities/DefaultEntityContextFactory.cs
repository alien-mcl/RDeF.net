using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RDeF.ComponentModel;
using RDeF.Mapping;
using RDeF.Reflection;

namespace RDeF.Entities
{
    /// <summary>Provides a default implementation of the <see cref="IEntityContextFactory" />.</summary>
    public sealed class DefaultEntityContextFactory : IEntityContextFactory
    {
        private readonly object _sync = new Object();
        private readonly ISet<Assembly> _mappingsAssemblies;
        private readonly IMappingsBuilder _mappingsBuilder;
        private IMappingsRepository _mappingsRepository;
        private Type _activatorType;
        private IActivator _activator;
        private bool _isActivatorCreatedManually;
        private Type _entitySourceType;
        private IEntitySource _entitySource;
        private IChangeDetector _changeDetector;
        private bool _isEntitySourceCreatedManually;

        /// <summary>Initializes a new instance of the <see cref="DefaultEntityContextFactory"/> class.</summary>
        internal DefaultEntityContextFactory()
        {
            _mappingsAssemblies = new HashSet<Assembly>();
            _mappingsBuilder = new DefaultMappingsBuilder(assembly =>
            {
                if (_mappingsAssemblies.Add(assembly))
                {
                    _mappingsRepository = null;
                }
            });
            _activatorType = typeof(DefaultActivator);
            _entitySourceType = typeof(SimpleInMemoryEntitySource);
        }

        private IMappingsRepository MappingsRepository
        {
            get
            {
                if (_mappingsRepository == null)
                {
                    lock (_sync)
                    {
                        var sources = from assembly in _mappingsAssemblies
                                      let context = new Dictionary<object, object>() { { typeof(IActivator), Activator }, { typeof(Assembly), assembly } }
                                      from mappingSource in ExecutionContext.ResolveAll<IMappingSource>(context)
                                      select mappingSource;
                        _mappingsRepository = new DefaultMappingRepository(sources);
                    }
                }

                return _mappingsRepository;
            }
        }

        private IActivator Activator
        {
            get
            {
                if (_activator == null)
                {
                    lock (_sync)
                    {
                        _activator = (IActivator)_activatorType.GetTypeInfo().GetConstructor(Type.EmptyTypes).Invoke(null);
                        _isActivatorCreatedManually = true;
                    }
                }

                return _activator;
            }
        }

        private IChangeDetector ChangeDetector
        {
            get
            {
                if (_changeDetector == null)
                {
                    _changeDetector = new DefaultChangeDetector(MappingsRepository);
                }

                return _changeDetector;
            }
        }

        private IEntitySource EntitySource
        {
            get
            {
                if (_entitySource == null)
                {
                    lock (_sync)
                    {
                        _entitySource = (IEntitySource)Activator.CreateInstance(_entitySourceType);
                        _isEntitySourceCreatedManually = true;
                    }
                }

                return _entitySource;
            }
        }

        /// <inheritdoc />
        public IEntityContext Create()
        {
            return new DefaultEntityContext(EntitySource, MappingsRepository, ChangeDetector);
        }

        /// <inheritdoc />
        public IEntityContextFactory WithActivator<T>()
        {
            if (!typeof(IActivator).IsAssignableFrom(typeof(T)))
            {
                throw new InvalidOperationException($"Unable to use type '{typeof(T)}' as activator as it doesn't implement type '{typeof(IActivator)}'.");
            }

            lock (_sync)
            {
                DisposeActivatorIfNeeded();
                _activatorType = typeof(T);
            }

            return this;
        }

        /// <inheritdoc />
        public IEntityContextFactory WithActivator(IActivator activator)
        {
            if (_activator == activator)
            {
                return this;
            }

            lock (_sync)
            {
                DisposeActivatorIfNeeded();
                _activator = activator;
            }

            return this;
        }

        /// <inheritdoc />
        public IEntityContextFactory WithEntitySource<T>()
        {
            if (!typeof(IEntitySource).IsAssignableFrom(typeof(T)))
            {
                throw new InvalidOperationException($"Unable to use type '{typeof(T)}' as entity source as it doesn't implement type '{typeof(IEntitySource)}'.");
            }

            lock (_sync)
            {
                DisposeEntitySourceIfNeeded();
                _entitySourceType = typeof(T);
            }

            return this;
        }

        /// <inheritdoc />
        public IEntityContextFactory WithEntitySource(IEntitySource entitySource)
        {
            if (_entitySource == entitySource)
            {
                return this;
            }

            lock (_sync)
            {
                DisposeEntitySourceIfNeeded();
                _entitySource = entitySource;
            }

            return this;
        }

        /// <inheritdoc />
        public IEntityContextFactory WithMappings(Action<IMappingsBuilder> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            lock (_sync)
            {
                builder(_mappingsBuilder);
            }

            return this;
        }

        private void DisposeActivatorIfNeeded()
        {
            if (_isActivatorCreatedManually)
            {
                (_activator as IDisposable)?.Dispose();
                _isActivatorCreatedManually = false;
            }

            _activator = null;
        }

        private void DisposeEntitySourceIfNeeded()
        {
            if (_isEntitySourceCreatedManually)
            {
                (_entitySource as IDisposable)?.Dispose();
                _isEntitySourceCreatedManually = false;
            }

            _entitySource = null;
        }
    }
}
