using System;
using System.Diagnostics.CodeAnalysis;
using RDeF.Entities;
using RDeF.Mapping;
using RDeF.Vocabularies;

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

            componentConfigurator.WithComponent<IChangeDetector, DefaultChangeDetector>(Lifestyle.BoundToEntityContext);
            componentConfigurator.WithComponent<IConverterProvider, DefaultConverterProvider>();
            componentConfigurator.WithInstance(new QIriMapping("xsd", xsd.Namespace), "xsd");
            componentConfigurator.WithInstance(new QIriMapping("rdf", rdf.Namespace), "rdf");
            componentConfigurator.WithInstance(new QIriMapping("rdfs", rdfs.Namespace), "rdfs");
            componentConfigurator.WithInstance(new QIriMapping("owl", owl.Namespace), "owl");
            componentConfigurator.WithInstance(new QIriMapping("oguid", oguid.Namespace), "oguid");
        }
    }
}
