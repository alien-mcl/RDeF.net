using System;
using System.Reflection;

namespace RDeF.Mapping.Explicit
{
    /// <summary>Describes an abstract explicit mappings provider.</summary>
    public interface IExplicitMappings
    {
        /// <summary>Sets an <paramref name="entityMapping" />.</summary>
        /// <param name="entityMapping">Entity mapping to set.</param>
        void Set(IEntityMapping entityMapping);

        /// <summary>Gets a explicitely set entity mapping.</summary>
        /// <param name="type">Type of the entity for which to obtain mapping.</param>
        /// <returns>Entity mapping matching a given <paramref name="type" /> or <b>null</b>.</returns>
        IEntityMapping FindEntityMappingFor(Type type);

        /// <summary>Gets a explicitely set property mapping.</summary>
        /// <param name="property">Property for which to obtain mapping.</param>
        /// <returns>Property mapping matching a given <paramref name="property" /> or <b>null</b>.</returns>
        IPropertyMapping FindPropertyMappingFor(PropertyInfo property);
    }
}
