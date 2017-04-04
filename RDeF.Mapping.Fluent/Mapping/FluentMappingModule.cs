using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using RDeF.ComponentModel;
using RDeF.Entities;
using RDeF.Mapping.Entities;
using RDeF.Mapping.Explicit;
using RDeF.Mapping.Visitors;

namespace RDeF.Mapping
{
    /// <summary>Registers fluent based mapping source provider.</summary>
    public class FluentMappingModule : IModule
    {
        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This is an IoC container initialization routine. No need to test it.")]
        public void Initialize(IComponentConfigurator componentConfigurator)
        {
            if (componentConfigurator == null)
            {
                throw new ArgumentNullException(nameof(componentConfigurator));
            }

            componentConfigurator.WithComponent<IChangeDetector, ExplicitMappingsAwareChangeDetector>(Lifestyle.BoundToEntityContext);
            componentConfigurator.WithComponent<IExplicitMappings, DefaultExplicitMappings>(
                Lifestyle.BoundToEntityContext,
                (container, explicitMappings) =>
                {
                    var context = container.Resolve<IEntityContext>();
                    EntityContextExtensions.ExplicitMappings[context] = explicitMappings;
                    if (EntityContextExtensions.LiteralConverters == null)
                    {
                        EntityContextExtensions.LiteralConverters = container.Resolve<IEnumerable<ILiteralConverter>>();
                        EntityContextExtensions.MappingVisitors = container.Resolve<IEnumerable<IMappingProviderVisitor>>();
                        EntityContextExtensions.QIriMappings = container.Resolve<IEnumerable<QIriMapping>>();
                    }

                    context.Disposed += (sender, args) => EntityContextExtensions.ExplicitMappings.Remove(context);
                });
        }
    }
}
