using System;
using System.Diagnostics.CodeAnalysis;
using RDeF.Entities;
using RDeF.Mapping;
using RDeF.Mapping.Visitors;

namespace RDeF.ComponentModel
{
    /// <summary>Registers base components.</summary>
    public class BaseComponentsModule : IModule
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

            componentConfigurator.WithComponent<IChangeDetector, DefaultChangeDetector>();
            componentConfigurator.WithComponent<IConverterProvider, DefaultConverterProvider>();
            componentConfigurator.WithMappingsProviderVisitor<CollectionStorageModelConventionVisitor>();
            componentConfigurator.WithMappingsProviderVisitor<ConverterConventionVisitor>();
        }
    }
}
