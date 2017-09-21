using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using RDeF.Collections;
using RDeF.Reflection;
using RollerCaster.Reflection;

namespace RDeF.ComponentModel
{
    internal sealed class SimpleContainer : IContainer
    {
        private const string AttributeMappingsAssembly = "RDeF.Mapping.Attributes";
        private const string FluentMappingsAssembly = "RDeF.Mapping.Fluent";

        private readonly ICollection<SimpleContainer> _childScopes = new List<SimpleContainer>();
        private readonly IDictionary<Type, ISet<IComponentRegistration>> _entityContextBoundServiceRegistrations =
            new ConcurrentDictionary<Type, ISet<IComponentRegistration>>();

        private readonly IDictionary<Type, ISet<IComponentRegistration>> _serviceRegistrations =
            new ConcurrentDictionary<Type, ISet<IComponentRegistration>>();

        private readonly IDictionary<Type, ISet<IComponentRegistration>> _instanceRegistrations =
            new ConcurrentDictionary<Type, ISet<IComponentRegistration>>();

        private readonly SimpleContainer _owner;
        private bool _areStandardLibrariesLoaded;

        internal SimpleContainer()
        {
        }

        internal SimpleContainer(SimpleContainer owner)
        {
            _owner = owner;
        }

        /// <summary>Begins a new scope.</summary>
        /// <returns>New scope container.</returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Instance is disposed correctly in DefaultEntityContextFactory.Create method.")]
        public SimpleContainer BeginScope()
        {
            var result = new SimpleContainer(this);
            result._areStandardLibrariesLoaded = _areStandardLibrariesLoaded;
            foreach (var type in _entityContextBoundServiceRegistrations.Keys)
            {
                foreach (var registration in _entityContextBoundServiceRegistrations[type])
                {
                    result._entityContextBoundServiceRegistrations.EnsureKey(type).Add(registration);
                }
            }

            _childScopes.Add(result);
            foreach (var serviceRegistration in _entityContextBoundServiceRegistrations)
            {
                var registrations = result._serviceRegistrations.EnsureKey(serviceRegistration.Key);
                foreach (var registration in serviceRegistration.Value)
                {
                    registrations.Add(registration);
                }
            }

            return result;
        }

        IContainer IContainer.BeginScope()
        {
            return BeginScope();
        }

        /// <inheritdoc />
        public bool IsRegistered<TService>()
        {
            return (_serviceRegistrations.ContainsKey(typeof(TService))) || (_instanceRegistrations.ContainsKey(typeof(TService)));
        }

        /// <inheritdoc />
        public void Register<TService>(Regex assemblyNamePattern = null)
        {
            EnsureStandardLibraries();
            foreach (var implementingType in CustomAttributeProviderExtensions.FindAllTypesImplementing<TService>(assemblyNamePattern))
            {
                Register<TService>(implementingType, implementingType.FullName);
            }
        }

        /// <inheritdoc />
        public IComponentRegistration Register<TService, TImplementation>(Lifestyle lifestyle = Lifestyle.Singleton) where TImplementation : TService
        {
            return Register<TService, TImplementation>(null, lifestyle);
        }

        /// <inheritdoc />
        public IComponentRegistration Register<TService, TImplementation>(string name, Lifestyle lifestyle = Lifestyle.Singleton) where TImplementation : TService
        {
            return Register<TService>(typeof(TImplementation), name, lifestyle);
        }

        /// <inheritdoc />
        public IComponentRegistration Register<TService>(Type implementationType, Lifestyle lifestyle = Lifestyle.Singleton)
        {
            return Register<TService>(implementationType, null, lifestyle);
        }

        /// <inheritdoc />
        public IComponentRegistration Register<TService>(Type implementationType, string name, Lifestyle lifestyle = Lifestyle.Singleton)
        {
            if (!typeof(TService).IsAssignableFrom(implementationType))
            {
                throw new ArgumentOutOfRangeException($"Unable to register type '{implementationType}' as a service '{typeof(TService)}'.");
            }

            var targetSetOfRegistration = (lifestyle == Lifestyle.BoundToEntityContext ? _entityContextBoundServiceRegistrations : _serviceRegistrations)
                .EnsureKey(typeof(TService));
            var result = new ComponentRegistration<TService>(implementationType, name);
            targetSetOfRegistration.Remove(result);
            targetSetOfRegistration.Add(result);
            return result;
        }

        /// <inheritdoc />
        public IComponentRegistration Register<TService>(TService instance, string name = null)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var result = new ComponentRegistration<TService>(instance, name);
            var targetSetOfRegistrations = _instanceRegistrations.EnsureKey(typeof(TService));
            targetSetOfRegistrations.Remove(result);
            targetSetOfRegistrations.Add(result);
            return result;
        }

        /// <inheritdoc />
        public TService Resolve<TService>()
        {
            return (TService)Resolve(typeof(TService));
        }

        /// <inheritdoc />
        public object Resolve(Type type)
        {
            var newInstances = new Dictionary<IComponentRegistration, object>();
            var instance = Resolve(type, new HashSet<Type>(), newInstances);
            foreach (var newInstance in newInstances)
            {
                newInstance.Key.OnActivate?.Invoke(this, newInstance.Value);
            }

            return instance;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            foreach (var instanceRegistration in _instanceRegistrations.Values.SelectMany(item => item))
            {
                (instanceRegistration.Instance as IDisposable)?.Dispose();
            }

            foreach (var childScope in _childScopes.ToList())
            {
                childScope.Dispose();
                childScope._owner._childScopes.Remove(childScope);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "It is expected that some assemblies will be missing, thus attempts to load them will fail, which needs suppresion.")]
        private static void TryLoadAssembly(AssemblyName assemblyName)
        {
            try
            {
                Assembly.Load(assemblyName);
            }
            catch
            {
                // Suppress any failed assembly load attempts.
            }
        }

        private object Resolve(Type serviceType, ISet<Type> visitedDependencies, IDictionary<IComponentRegistration, object> newInstances)
        {
            if (serviceType.IsADirectEnumerableType())
            {
                return ResolveAll(serviceType, visitedDependencies, newInstances);
            }

            ISet<IComponentRegistration> instanceRegistration;
            if (_instanceRegistrations.TryGetValue(serviceType, out instanceRegistration))
            {
                return instanceRegistration.OrderBy(item => item.Name?.Length ?? -1).First().Instance;
            }

            ISet<IComponentRegistration> serviceRegistration;
            if ((!_serviceRegistrations.TryGetValue(serviceType, out serviceRegistration)) || (serviceRegistration.Count == 0))
            {
                return _owner?.Resolve(serviceType, new HashSet<Type>(), newInstances);
            }

            return BuildInstance(serviceRegistration, serviceType, serviceRegistration.OrderBy(item => item.Name?.Length ?? -1).First(), visitedDependencies, newInstances);
        }

        private IEnumerable ResolveAll(Type serviceType, ISet<Type> visitedDependencies, IDictionary<IComponentRegistration, object> newInstances)
        {
            var itemType = serviceType.GetItemType();
            IList result = (IList)typeof(List<>).MakeGenericType(itemType).GetConstructor(Type.EmptyTypes).Invoke(null);
            ISet<IComponentRegistration> instanceRegistrations;
            if (_instanceRegistrations.TryGetValue(itemType, out instanceRegistrations))
            {
                foreach (var instanceRegistration in instanceRegistrations)
                {
                    result.Add(instanceRegistration.Instance);
                }
            }

            ISet<IComponentRegistration> serviceRegistration;
            if (_serviceRegistrations.TryGetValue(itemType, out serviceRegistration))
            {
                foreach (var implementationRegistered in serviceRegistration.ToList())
                {
                    var instance = BuildInstance(serviceRegistration, itemType, implementationRegistered, visitedDependencies, newInstances);
                    if (instance != null)
                    {
                        result.Add(instance);
                    }
                }
            }

            if (_owner != null)
            {
                foreach (var instance in _owner.ResolveAll(serviceType, new HashSet<Type>(), newInstances))
                {
                    result.Add(instance);
                }
            }

            return result;
        }

        private object BuildInstance(
            ISet<IComponentRegistration> serviceRegistration,
            Type serviceType,
            IComponentRegistration implementationRegistered,
            ISet<Type> visitedDependencies,
            IDictionary<IComponentRegistration, object> newInstances)
        {
            var result = CreateInstance(implementationRegistered, visitedDependencies, newInstances);
            if (result == null)
            {
                return null;
            }

            serviceRegistration.Remove(implementationRegistered);
            var instanceRegistration = (IComponentRegistration)typeof(ComponentRegistration<>).MakeGenericType(serviceType)
                .GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { serviceType, typeof(string) }, null)
                .Invoke(new[] { result, implementationRegistered.Name });
            _instanceRegistrations.EnsureKey(serviceType).Add(instanceRegistration);
            return result;
        }

        private object CreateInstance(IComponentRegistration type, ISet<Type> visitedDependencies, IDictionary<IComponentRegistration, object> newInstances)
        {
            if (visitedDependencies.Contains(type.ImplementationType))
            {
                throw new InvalidOperationException($"Dependency loop detected for type '{type}'");
            }

            visitedDependencies.Add(type.ImplementationType);
            foreach (var ctor in type.ImplementationType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).OrderByDescending(ctor => ctor.GetParameters().Length))
            {
                bool canCreateInstance = true;
                List<object> arguments = null;
                foreach (var parameter in ctor.GetParameters())
                {
                    object value = (parameter.HasDefaultValue ? parameter.DefaultValue : null);
                    object resolvedInstance = Resolve(parameter.ParameterType, visitedDependencies, newInstances);
                    if ((resolvedInstance != null) || (parameter.HasDefaultValue))
                    {
                        (arguments ?? (arguments = new List<object>())).Add(resolvedInstance ?? value);
                    }
                    else
                    {
                        canCreateInstance = false;
                        break;
                    }
                }

                if (!canCreateInstance)
                {
                    continue;
                }

                var result = ctor.Invoke(arguments?.ToArray());
                newInstances[type] = result;
                return result;
            }

            visitedDependencies.Remove(type.ImplementationType);
            return null;
        }

        private void EnsureStandardLibraries()
        {
            if (_areStandardLibrariesLoaded)
            {
                return;
            }

            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            if (loadedAssemblies.All(assembly => assembly.GetName().Name != AttributeMappingsAssembly))
            {
                TryLoadAssembly(new AssemblyName(AttributeMappingsAssembly));
            }

            if (loadedAssemblies.All(assembly => assembly.GetName().Name != FluentMappingsAssembly))
            {
                TryLoadAssembly(new AssemblyName(FluentMappingsAssembly));
            }

            _areStandardLibrariesLoaded = true;
        }
    }
}
