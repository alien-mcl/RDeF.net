using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using RDeF.Mapping;

namespace RDeF.Reflection
{
    [ExcludeFromCodeCoverage]
    internal static class ExecutionContext
    {
        private const string AttributeMappingsAssembly = "RDeF.Mapping.Attributes";
        private static readonly IDictionary<Type, IEnumerable<Type>> TypeImplementations = new ConcurrentDictionary<Type, IEnumerable<Type>>();

        internal static IEnumerable<T> ResolveAll<T>(IDictionary<object, object> context)
        {
            return from type in FindAllTypesImplementing<T>()
                   let instance = (T)type.CreateInstance(context)
                   where instance != null
                   select instance;
        }

        private static object CreateInstance(this Type type, IDictionary<object, object> context)
        {
            foreach (var ctor in type.GetTypeInfo().GetConstructors(BindingFlags.Instance | BindingFlags.Public).OrderByDescending(ctor => ctor.GetParameters().Length))
            {
                bool canCreateInstance = true;
                List<object> parameters = null;
                foreach (var parameter in ctor.GetParameters())
                {
                    object value = (parameter.HasDefaultValue ? parameter.DefaultValue : null);
                    if ((context.TryGetValue(parameter.ParameterType, out value)) || (context.TryGetValue(parameter.Name, out value)) || (parameter.HasDefaultValue))
                    {
                        (parameters ?? (parameters = new List<object>())).Add(value);
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

                return ctor.Invoke(parameters?.ToArray());
            }

            return null;
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Assemblies with incomplete dependencies would throw, which is not the expected behavior at that stage.")]
        private static IEnumerable<Type> FindAllTypesImplementing<T>()
        {
            IEnumerable<Type> result;
            if (TypeImplementations.TryGetValue(typeof(T), out result))
            {
                return result;
            }

            ISet<Type> implementations = new HashSet<Type>();
            TypeImplementations[typeof(T)] = result = implementations;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().WithStandardLibraries().Where(assembly => !assembly.IsDynamic))
            {
                try
                {
                    var types = from type in assembly.GetExportedTypes()
                                where type.IsValidMappingSourceType()
                                select type;
                    foreach (var type in types)
                    {
                        implementations.Add(type);
                    }
                }
                catch
                {
                    //// Suppress any exceptions thrown.
                }
            }

            return result;
        }

        private static bool IsValidMappingSourceType(this Type type)
        {
            return !type.IsValueType && !type.IsInterface && !type.IsEnum && !type.IsAbstract && !type.IsArray && typeof(IMappingSource).IsAssignableFrom(type);
        }

        private static IEnumerable<Assembly> WithStandardLibraries(this IEnumerable<Assembly> loadedAssemblies)
        {
            if (loadedAssemblies.Any(assembly => assembly.GetName().Name == AttributeMappingsAssembly))
            {
                return loadedAssemblies;
            }

            var mappingAssembly = Assembly.Load(new AssemblyName(AttributeMappingsAssembly));
            return (mappingAssembly != null ? loadedAssemblies.Concat(new[] { mappingAssembly }) : loadedAssemblies);
        }
    }
}