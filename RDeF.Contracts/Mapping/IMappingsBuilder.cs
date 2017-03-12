using System.Diagnostics.CodeAnalysis;

namespace RDeF.Mapping
{
    /// <summary>Describes an abstract mappings builder.</summary>
    public interface IMappingsBuilder
    {
        /// <summary>Registers mappings from the assembly of <typeparamref name="T" />.</summary>
        /// <typeparam name="T">Type that points to the mappings assembly.</typeparam>
        /// <returns>Current instance of the <see cref="IMappingsBuilder" />.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "This is a part of a fluent-like API.")]
        IMappingsBuilder FromAssemblyOf<T>();
    }
}
