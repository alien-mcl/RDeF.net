using System;

namespace RDeF.Mapping.Providers
{
    /// <summary>Describes a dictionary mapping provider.</summary>
    public interface IDictionaryMappingProvider : IPropertyMappingProvider
    {
        /// <summary>Gets the key converter type.</summary>
        Type KeyConverterType { get; }
    }
}
