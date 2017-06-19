using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RDeF.Reflection
{
    internal static class TypeExtensions
    {
        [SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", Justification = "Strings are culture invariant.")]
        internal static Stream GetEmbeddedResource(this Type type, string extension, Func<string, string> transformation = null)
        {
            if (transformation == null)
            {
                transformation = name => name;
            }

            var assembly = type.GetTypeInfo().Assembly;
            var resourceName = (
                from name in assembly.GetManifestResourceNames()
                where name.Replace("\\", ".").Contains(transformation(type.FullName)) && name.EndsWith(extension)
                select name).First();

            return assembly.GetManifestResourceStream(resourceName);
        }
    }
}
