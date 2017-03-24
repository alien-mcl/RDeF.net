using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
        private readonly IDictionary<Type, IDictionary<Type, Type>> _serviceRegistrations = new ConcurrentDictionary<Type, IDictionary<Type, Type>>();
        private readonly IDictionary<Type, IDictionary<Type, object>> _instanceRegistrations = new ConcurrentDictionary<Type, IDictionary<Type, object>>();
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
        public SimpleContainer BeginScope()
        {
            return new SimpleContainer(this);
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
        public void Register<TService, TImplementation>() where TImplementation : TService
        {
            Register<TService>(typeof(TImplementation));
        }

        /// <inheritdoc />
        public void Register<TService>(Type implementationType)
        {
            if (!typeof(TService).IsAssignableFrom(implementationType))
            {
                throw new ArgumentOutOfRangeException($"Unable to register type '{implementationType}' as a service '{typeof(TService)}'.");
            }

            _serviceRegistrations.EnsureKey(typeof(TService), true)[implementationType] = implementationType;
        }

        /// <inheritdoc />
        public void Register<TService>(TService instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            _instanceRegistrations.EnsureKey(typeof(TService), true)[instance.GetType()] = instance;
        }

        /// <inheritdoc />
        public void Unregister<TService>()
        {
            _serviceRegistrations.Remove(typeof(TService));
            IDictionary<Type, object> instanceRegistration;
            if (!_instanceRegistrations.TryGetValue(typeof(TService), out instanceRegistration))
            {
                return;
            }

            _instanceRegistrations.Remove(typeof(TService));
            foreach (var instance in instanceRegistration.Values)
            {
                (instance as IDisposable)?.Dispose();
            }
        }

        /// <inheritdoc />
        public void Unregister<TService>(TService instance)
        {
            if (instance == null)
            {
                return;
            }

            IDictionary<Type, object> instanceRegistration;
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
            return (TService)Resolve(typeof(TService), new HashSet<Type>());
        }

        /// <inheritdoc />
        public void Dispose()
        {
            foreach (var serviceRegistration in _instanceRegistrations)
            {
                foreach (var instanceRegistration in serviceRegistration.Value)
                {
                    (instanceRegistration.Value as IDisposable)?.Dispose();
                }
            }
        }

        private object Resolve(Type serviceType, ISet<Type> visitedDependencies)
        {
            if (serviceType.IsADirectEnumerableType())
            {
                return ResolveAll(serviceType, visitedDependencies);
            }

            IDictionary<Type, object> instanceRegistration;
            if (_instanceRegistrations.TryGetValue(serviceType, out instanceRegistration))
            {
                return instanceRegistration.Values.First();
            }

            IDictionary<Type, Type> serviceRegistration;
            if ((!_serviceRegistrations.TryGetValue(serviceType, out serviceRegistration)) || (serviceRegistration.Count == 0))
            {
                return _owner?.Resolve(serviceType, new HashSet<Type>());
            }

            return BuildInstance(serviceRegistration, serviceType, serviceRegistration.Values.First(), visitedDependencies);
        }

        private IEnumerable ResolveAll(Type serviceType, ISet<Type> visitedDependencies)
        {
            var itemType = serviceType.GetItemType();
            IList result = (IList)typeof(List<>).MakeGenericType(itemType).GetTypeInfo().GetConstructor(Type.EmptyTypes).Invoke(null);
            IDictionary<Type, object> instanceRegistrations;
            if (_instanceRegistrations.TryGetValue(itemType, out instanceRegistrations))
            {
                foreach (var instanceRegistration in instanceRegistrations)
                {
                    result.Add(instanceRegistration.Value);
                }
            }

            IDictionary<Type, Type> serviceRegistration;
            if (!_serviceRegistrations.TryGetValue(itemType, out serviceRegistration))
            {
                return result;
            }

            foreach (var implementationRegistered in serviceRegistration.Keys.ToArray())
            {
                var instance = BuildInstance(serviceRegistration, itemType, implementationRegistered, visitedDependencies);
                if (instance != null)
                {
                    result.Add(instance);
                }
            }

            if (_owner != null)
            {
                foreach (var instance in _owner.ResolveAll(serviceType, new HashSet<Type>()))
                {
                    result.Add(instance);
                }
            }

            return result;
        }

        private object BuildInstance(
            IDictionary<Type, Type> serviceRegistration,
            Type serviceType,
            Type implementationRegistered,
            ISet<Type> visitedDependencies)
        {
            var result = CreateInstance(implementationRegistered, visitedDependencies);
            if (result == null)
            {
                return null;
            }

            serviceRegistration.Remove(result.GetType());
            _instanceRegistrations.EnsureKey(serviceType, true)[result.GetType()] = result;
            return result;
        }

        private object CreateInstance(Type type, ISet<Type> visitedDependencies)
        {
            if (visitedDependencies.Contains(type))
            {
                throw new InvalidOperationException($"Dependency loop detected for type '{type}'");
            }

            visitedDependencies.Add(type);
            foreach (var ctor in type.GetTypeInfo().GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).OrderByDescending(ctor => ctor.GetParameters().Length))
            {
                bool canCreateInstance = true;
                List<object> arguments = null;
                foreach (var parameter in ctor.GetParameters())
                {
                    object value = (parameter.HasDefaultValue ? parameter.DefaultValue : null);
                    object resolvedInstance = Resolve(parameter.ParameterType, visitedDependencies);
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

                return ctor.Invoke(arguments?.ToArray());
            }

            visitedDependencies.Remove(type);
            return null;
        }

        private void EnsureStandardLibraries()
        {
            if (_areStandardLibrariesLoaded)
            {
                return;
            }

            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            if (loadedAssemblies.Any(assembly => assembly.GetName().Name == AttributeMappingsAssembly))
            {
                return;
            }

            Assembly.Load(new AssemblyName(AttributeMappingsAssembly));
            _areStandardLibrariesLoaded = true;
        }
    }
}
