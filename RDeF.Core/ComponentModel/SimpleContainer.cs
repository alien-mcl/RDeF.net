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
#if NETSTANDARD1_6
using Microsoft.Extensions.DependencyModel;
#endif

namespace RDeF.ComponentModel
{
    internal sealed class SimpleContainer : IContainer
    {
        private const string AttributeMappingsAssembly = "RDeF.Mapping.Attributes";
        private readonly IDictionary<Type, IDictionary<Type, IComponentRegistration>> _entityContextBoundServiceRegistrations =
            new ConcurrentDictionary<Type, IDictionary<Type, IComponentRegistration>>();

        private readonly IDictionary<Type, IDictionary<Type, IComponentRegistration>> _serviceRegistrations =
            new ConcurrentDictionary<Type, IDictionary<Type, IComponentRegistration>>();

        private readonly IDictionary<Type, IDictionary<Type, IComponentRegistration>> _instanceRegistrations =
            new ConcurrentDictionary<Type, IDictionary<Type, IComponentRegistration>>();

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
            foreach (var serviceRegistration in _entityContextBoundServiceRegistrations)
            {
                var registrations = result._serviceRegistrations.EnsureKey(serviceRegistration.Key);
                foreach (var registration in serviceRegistration.Value)
                {
                    registrations[registration.Key] = registration.Value;
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
                Register<TService>(implementingType);
            }
        }

        /// <inheritdoc />
        public IComponentRegistration Register<TService, TImplementation>(Lifestyle lifestyle = Lifestyle.Singleton) where TImplementation : TService
        {
            return Register<TService>(typeof(TImplementation), lifestyle);
        }

        /// <inheritdoc />
        public IComponentRegistration Register<TService>(Type implementationType, Lifestyle lifestyle = Lifestyle.Singleton)
        {
            if (!typeof(TService).IsAssignableFrom(implementationType))
            {
                throw new ArgumentOutOfRangeException($"Unable to register type '{implementationType}' as a service '{typeof(TService)}'.");
            }

            return (lifestyle == Lifestyle.BoundToEntityContext ? _entityContextBoundServiceRegistrations : _serviceRegistrations)
                .EnsureKey(typeof(TService), true)[implementationType] = new ComponentRegistration<TService>(implementationType);
        }

        /// <inheritdoc />
        public IComponentRegistration Register<TService>(TService instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return _instanceRegistrations.EnsureKey(typeof(TService), true)[instance.GetType()] = new ComponentRegistration<TService>(instance);
        }

        /// <inheritdoc />
        public void Unregister<TService>()
        {
            _serviceRegistrations.Remove(typeof(TService));
            IDictionary<Type, IComponentRegistration> instanceRegistration;
            if (!_instanceRegistrations.TryGetValue(typeof(TService), out instanceRegistration))
            {
                return;
            }

            _instanceRegistrations.Remove(typeof(TService));
            foreach (var registration in instanceRegistration.Values)
            {
                (registration.Instance as IDisposable)?.Dispose();
            }
        }

        /// <inheritdoc />
        public void Unregister<TService>(TService instance)
        {
            if (instance == null)
            {
                return;
            }

            IDictionary<Type, IComponentRegistration> instanceRegistration;
            if (!_instanceRegistrations.TryGetValue(typeof(TService), out instanceRegistration))
            {
                return;
            }

            instanceRegistration.Remove(instance.GetType());
            (instance as IDisposable)?.Dispose();
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
            foreach (var serviceRegistration in _instanceRegistrations)
            {
                foreach (var instanceRegistration in serviceRegistration.Value)
                {
                    (instanceRegistration.Value.Instance as IDisposable)?.Dispose();
                }
            }
        }

        private object Resolve(Type serviceType, ISet<Type> visitedDependencies, IDictionary<IComponentRegistration, object> newInstances)
        {
            if (serviceType.IsADirectEnumerableType())
            {
                return ResolveAll(serviceType, visitedDependencies, newInstances);
            }

            IDictionary<Type, IComponentRegistration> instanceRegistration;
            if (_instanceRegistrations.TryGetValue(serviceType, out instanceRegistration))
            {
                return instanceRegistration.Values.First().Instance;
            }

            IDictionary<Type, IComponentRegistration> serviceRegistration;
            if ((!_serviceRegistrations.TryGetValue(serviceType, out serviceRegistration)) || (serviceRegistration.Count == 0))
            {
                return _owner?.Resolve(serviceType, new HashSet<Type>(), newInstances);
            }

            return BuildInstance(serviceRegistration, serviceType, serviceRegistration.Values.First(), visitedDependencies, newInstances);
        }

        private IEnumerable ResolveAll(Type serviceType, ISet<Type> visitedDependencies, IDictionary<IComponentRegistration, object> newInstances)
        {
            var itemType = serviceType.GetItemType();
            IList result = (IList)typeof(List<>).MakeGenericType(itemType).GetConstructor(Type.EmptyTypes).Invoke(null);
            IDictionary<Type, IComponentRegistration> instanceRegistrations;
            if (_instanceRegistrations.TryGetValue(itemType, out instanceRegistrations))
            {
                foreach (var instanceRegistration in instanceRegistrations)
                {
                    result.Add(instanceRegistration.Value.Instance);
                }
            }

            IDictionary<Type, IComponentRegistration> serviceRegistration;
            if (_serviceRegistrations.TryGetValue(itemType, out serviceRegistration))
            {
                foreach (var implementationRegistered in serviceRegistration)
                {
                    var instance = BuildInstance(serviceRegistration, itemType, implementationRegistered.Value, visitedDependencies, newInstances);
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
            IDictionary<Type, IComponentRegistration> serviceRegistration,
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

            serviceRegistration.Remove(result.GetType());
#if NETSTANDARD1_6
            _instanceRegistrations.EnsureKey(serviceType, true)[result.GetType()] = (IComponentRegistration)typeof(ComponentRegistration<>).MakeGenericType(serviceType)
                .GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                .First(ctor => ctor.GetParameters().Length == 0 && ctor.GetParameters()[0].ParameterType == serviceType)
#else

            _instanceRegistrations.EnsureKey(serviceType, true)[result.GetType()] = (IComponentRegistration)typeof(ComponentRegistration<>).MakeGenericType(serviceType)
                .GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { serviceType }, null)
#endif
                .Invoke(new[] { result });
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
#if NETSTANDARD1_6
            var loadedAssemblies = DependencyContext.Default.RuntimeLibraries
                .SelectMany(library => library.Assemblies)
                .Select(assembly => Assembly.Load(assembly.Name));
#else
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
#endif
            if (loadedAssemblies.Any(assembly => assembly.GetName().Name == AttributeMappingsAssembly))
            {
                return;
            }

            Assembly.Load(new AssemblyName(AttributeMappingsAssembly));
            _areStandardLibrariesLoaded = true;
        }
    }
}
