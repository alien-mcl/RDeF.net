using System.Diagnostics.CodeAnalysis;

namespace RDeF.Mapping.Providers
{
    /// <summary>Represents an entity mapping.</summary>
    [SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces", Justification = "Having an attribute would be inconsistant with current API.")]
    public interface IEntityMappingProvider : ITermMappingProvider
    {
    }
}
