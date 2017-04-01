using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using RDeF.ComponentModel;
using RDeF.Mapping;
using RDeF.Mapping.Visitors;

namespace RDeF.Entities
{
    /// <summary>Provides a default implementation of the <see cref="IEntityContextFactory" />.</summary>
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Instances are disposed correctly.")]
    public sealed class DefaultEntityContextFactory : IEntityContextFactory, IComponentConfigurator
    {
        private readonly object _sync = new object();
        private readonly IMappingsBuilder _mappingsBuilder;
        private readonly IContainer _container;

        /// <summary>Initializes a new instance of the <see cref="DefaultEntityContextFactory"/> class.</summary>
        public DefaultEntityContextFactory()
        {
            var mappingsAssemblies = new HashSet<Assembly>();
            _mappingsBuilder = new DefaultMappingsBuilder(assembly =>
            {
                if (!mappingsAssemblies.Add(assembly))
                {
                    return;
                }

                var mappingSources = from mappingSourceProvider in _container.Resolve<IEnumerable<IMappingSourceProvider>>()
                                     from mappingSource in mappingSourceProvider.GetMappingSourcesFor(assembly)
                                     select mappingSource;
                foreach (var mappingSource in mappingSources)
                {
                    _container.Register(mappingSource);
                }

                _container.Unregister<IMappingBuilder>();
                _container.Unregister<IMappingsRepository>();
                _container.Register<IMappingBuilder, DefaultMappingBuilder>();
                _container.Register<IMappingsRepository, DefaultMappingRepository>();
            });
            _container = new SimpleContainer();
            _container.Register<IChangeDetector, DefaultChangeDetector>();
            _container.Register<IMappingBuilder, DefaultMappingBuilder>();
            _container.Register<IMappingsRepository, DefaultMappingRepository>();
            _container.Register<IModule>(new Regex("^RDeF\\.*"));
            foreach (var module in _container.Resolve<IEnumerable<IModule>>())
            {
                module.Initialize(this);
            }
        }

        /// <inheritdoc />
        public IEntityContext Create()
        {
            var scope = _container.BeginScope();
            scope.Register<IEntityContext, DefaultEntityContext>();
            if (!scope.IsRegistered<IEntitySource>())
            {
                scope.Register<IEntitySource, SimpleInMemoryEntitySource>();
            }

            var result = scope.Resolve<IEntityContext>();
            result.Disposed += (sender, e) => scope.Dispose();
            return result;
        }

        /// <inheritdoc />
        public IEntityContextFactory WithEntitySource<T>() where T : IEntitySource
        {
            if (!typeof(IEntitySource).IsAssignableFrom(typeof(T)))
            {
                throw new InvalidOperationException($"Unable to use type '{typeof(T)}' as entity source as it doesn't implement type '{typeof(IEntitySource)}'.");
            }

            _container.Unregister<IEntitySource>();
            _container.Register<IEntitySource, T>();
            return this;
        }

        /// <inheritdoc />
        public IEntityContextFactory WithEntitySource(IEntitySource entitySource)
        {
            _container.Unregister<IEntitySource>();
            _container.Register(entitySource);
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

        /// <inheritdoc />
        public IEntityContextFactory WithModule<TModule>() where TModule : IModule
        {
            ((IModule)typeof(TModule).GetConstructor(Type.EmptyTypes).Invoke(null)).Initialize(this);
            return this;
        }

        /// <inheritdoc />
        IComponentConfigurator IComponentConfigurator.WithConverter<TConverter>()
        {
            _container.Register<ILiteralConverter, TConverter>();
            return this;
        }

        /// <inheritdoc />
        IComponentConfigurator IComponentConfigurator.WithMappingsProvidedBy<TMappingSourceProvider>()
        {
            _container.Register<IMappingSourceProvider, TMappingSourceProvider>();
            return this;
        }

        IComponentConfigurator IComponentConfigurator.WithMappingsProviderVisitor<TMappingProviderVisitor>()
        {
            _container.Register<IMappingProviderVisitor, TMappingProviderVisitor>();
            return this;
        }
    }
}
