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
                    _container.Register(mappingSource, $"{mappingSource.GetType()}_{mappingSource.GetHashCode()}");
                }
            });
            _container = new SimpleContainer();
            _container.Register<IMappingBuilder, DefaultMappingBuilder>();
            _container.Register<IMappingsRepository, DefaultMappingsRepository>();
            _container.Register<IModule>(new Regex("^RDeF\\.*", RegexOptions.IgnoreCase));
            foreach (var module in _container.Resolve<IEnumerable<IModule>>())
            {
                module.Initialize(this);
            }
        }

        /// <summary>Finalizes an instance of the <see cref="DefaultEntityContextFactory"/> class.</summary>
        ~DefaultEntityContextFactory()
        {
            _container.Dispose();
        }

        /// <inheritdoc />
        public IMappingsRepository Mappings
        {
            get { return _container.Resolve<IMappingsRepository>(); }
        }

        /// <inheritdoc />
        public IEntityContext Create()
        {
            WithMappings(_ => _.FromAssemblyOf<ITypedEntity>());
            var scope = _container.BeginScope();
            scope.Register<IEntityContext, DefaultEntityContext>();
            scope.Register<Func<DefaultEntityContext>>(() => (DefaultEntityContext)scope.Resolve<IEntityContext>());
            scope.Register<Func<IEntityContext>>(() => scope.Resolve<IEntityContext>());
            if (!_container.IsRegistered<IEntitySource>())
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
            _container.Register<IEntitySource, T>();
            return this;
        }

        /// <inheritdoc />
        public IEntityContextFactory WithEntitySource(IEntitySource entitySource)
        {
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
        public IEntityContextFactory WithQIri(string prefix, Iri iri)
        {
            _container.Register(new QIriMapping(prefix, iri), prefix);
            return this;
        }

        /// <inheritdoc />
        public IEntityContextFactory WithModule<TModule>() where TModule : IModule
        {
            ((IModule)typeof(TModule).GetConstructor(Type.EmptyTypes).Invoke(null)).Initialize(this);
            return this;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _container.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        IComponentConfigurator IComponentConfigurator.WithConverter<TConverter>()
        {
            _container.Register<ILiteralConverter, TConverter>(typeof(TConverter).FullName);
            return this;
        }

        /// <inheritdoc />
        IComponentConfigurator IComponentConfigurator.WithMappingsProvidedBy<TMappingSourceProvider>()
        {
            _container.Register<IMappingSourceProvider, TMappingSourceProvider>(typeof(TMappingSourceProvider).FullName);
            return this;
        }

        /// <inheritdoc />
        IComponentConfigurator IComponentConfigurator.WithMappingsProviderVisitor<TMappingProviderVisitor>()
        {
            _container.Register<IMappingProviderVisitor, TMappingProviderVisitor>(typeof(TMappingProviderVisitor).FullName);
            return this;
        }

        /// <inheritdoc />
        IComponentConfigurator IComponentConfigurator.WithComponent<TService, TComponent>(Lifestyle lifestyle, Action<IComponentScope, TComponent> onActivate)
        {
            return ((IComponentConfigurator)this).WithComponent<TService, TComponent>(null, lifestyle, onActivate);
        }

        /// <inheritdoc />
        IComponentConfigurator IComponentConfigurator.WithComponent<TService, TComponent>(string name, Lifestyle lifestyle, Action<IComponentScope, TComponent> onActivate)
        {
            var registration = _container.Register<TService, TComponent>(name, lifestyle);
            if (onActivate != null)
            {
                registration.OnActivate = (container, instance) => onActivate(container, (TComponent)instance);
            }

            return this;
        }

        /// <inheritdoc />
        IComponentConfigurator IComponentConfigurator.WithInstance<TService>(TService instance, string name)
        {
            _container.Register(instance, name);
            return this;
        }
    }
}
