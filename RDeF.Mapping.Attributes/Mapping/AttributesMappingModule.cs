using System;
using System.Diagnostics.CodeAnalysis;
using RDeF.ComponentModel;

namespace RDeF.Mapping
{
    /// <summary>Registers attribute based mapping source provider.</summary>
    public class AttributesMappingModule : IModule
    {
        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [SuppressMessage("TS0000", "NoUnitTests", Justification = "This is an IoC container initialization routine. No need to test it.")]
        public void Initialize(IComponentConfigurator componentConfigurator)
        {
            if (componentConfigurator == null)
            {
                throw new ArgumentNullException(nameof(componentConfigurator));
            }

            componentConfigurator.WithMappingsProvidedBy<AttributesMappingSourceProvider>();
        }
    }
}
