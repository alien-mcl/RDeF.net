using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace RDeF.Reflection
{
    internal static class CustomAttributeProviderExtensions
    {
        private static readonly Type[] EnumerableTypes = { typeof(IEnumerable<>), typeof(ICollection<>), typeof(IList<>) };
        private static readonly IDictionary<Type, ISet<Type>> TypeImplementations = new ConcurrentDictionary<Type, ISet<Type>>();

        internal static bool IsADirectEnumerableType(this Type type)
        {
            if (type.IsArray)
            {
                return true;
            }

            if (!type.IsGenericType)
            {
                return false;
            }

            var genericTypeDefition = type.GetGenericTypeDefinition();
            return EnumerableTypes.Contains(genericTypeDefition);
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Assemblies with incomplete dependencies would throw, which is not the expected behavior at that stage.")]
        internal static IEnumerable<Type> FindAllTypesImplementing<T>(Regex assemblyNamePattern = null)
        {
            ISet<Type> result;
            if (TypeImplementations.TryGetValue(typeof(T), out result))
            {
                return result;
            }

            TypeImplementations[typeof(T)] = result = new HashSet<Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => (!assembly.IsDynamic) && ((assemblyNamePattern == null) || (assemblyNamePattern.IsMatch(assembly.FullName)))))
            {
                try
                {
                    assembly.FindAllTypesImplementing<T>();
                }
                catch
                {
                    //// Suppress any exceptions thrown.
                }
            }

            return result;
        }

        private static void FindAllTypesImplementing<T>(this Assembly assembly)
        {
            var implementations = TypeImplementations[typeof(T)];
            var types = from type in assembly.GetExportedTypes()
                        where type.IsValidImplementourOf<T>()
                        select type;
            foreach (var type in types)
            {
                implementations.Add(type);
            }
        }

        private static bool IsValidImplementourOf<T>(this Type implementationType)
        {
            return !implementationType.IsValueType &&
                !implementationType.IsInterface &&
                !implementationType.IsEnum &&
                !implementationType.IsAbstract &&
                !implementationType.IsArray &&
                typeof(T).IsAssignableFrom(implementationType);
        }
    }
}