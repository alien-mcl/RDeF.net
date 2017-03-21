using System;
using System.Reflection;

namespace RDeF.Mapping.Providers
{
    /// <summary>Represents an abstract property mapping provider.</summary>
    public interface IPropertyMappingProvider : ITermMappingProvider
    {
        /// <summary>Gets the property being mapped.</summary>
        PropertyInfo Property { get; }

        /// <summary>Gets or sets the value converter type the mapping.</summary>
        Type ValueConverterType { get; set; }
    }
}