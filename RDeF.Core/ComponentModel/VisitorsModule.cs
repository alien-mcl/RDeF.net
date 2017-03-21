using System;
using System.Diagnostics.CodeAnalysis;
using RDeF.Mapping.Visitors;

namespace RDeF.ComponentModel
{
    /// <summary>Registers default visitors.</summary>
    public class VisitorsModule : IModule
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

            componentConfigurator.WithMappingsProviderVisitor<CollectionStorageModelConventionVisitor>();
            componentConfigurator.WithMappingsProviderVisitor<ConverterConventionVisitor>();
        }
    }
}
