#if !NETSTANDARD2_0
using System.Configuration;
#endif
using System.Diagnostics.CodeAnalysis;
using RDeF.Entities;

namespace RDeF.Configuration
{
    /// <summary>Describes an <see cref="IEntityContextFactory" /> configuration.</summary>
    [ExcludeFromCodeCoverage]
    [SuppressMessage("TS0000", "NoUnitTests", Justification = "Simple configuration wrapper without special logic to be tested.")]
    public class FactoryConfigurationElement
#if !NETSTANDARD2_0
        : ConfigurationElement
#endif
    {
#if NETSTANDARD2_0
        private QIriConfigurationElementCollection _qIris;
        private MappingAssemblyConfigurationElementCollection _mappingAssemblies;

        /// <summary>Initializes a new instance of the <see cref="FactoryConfigurationElement" /> class.</summary>
        public FactoryConfigurationElement()
        {
            _qIris = new QIriConfigurationElementCollection();
            _mappingAssemblies = new MappingAssemblyConfigurationElementCollection();
        }
#else
        private const string NameAttributeName = "name";
        private const string MappingAssembliesAttributeName = "mappingAssemblies";
        private const string QIrisAttributeName = "qiris";
#endif

        /// <summary>Gets a name of the factory configuration.</summary>
#if !NETSTANDARD2_0
        [ConfigurationProperty(NameAttributeName, IsKey = true, IsRequired = true)]
#endif
        public string Name
        {
#if NETSTANDARD2_0
            get;

            set;
#else
            get { return (string)this[NameAttributeName]; }

            internal set { this[NameAttributeName] = value; }
#endif
        }

        /// <summary>Gets a collection of mapping assemblies.</summary>
#if !NETSTANDARD2_0
        [ConfigurationProperty(MappingAssembliesAttributeName)]
#endif
        public MappingAssemblyConfigurationElementCollection MappingAssemblies
        {
#if NETSTANDARD2_0
            get { return _mappingAssemblies; }

            set { _mappingAssemblies = value ?? new MappingAssemblyConfigurationElementCollection(); }
#else
            get { return (MappingAssemblyConfigurationElementCollection)this[MappingAssembliesAttributeName]; }

            internal set { this[MappingAssembliesAttributeName] = value; }
#endif
        }

        /// <summary>Gets a QIri mappings.</summary>
#if !NETSTANDARD2_0
        [ConfigurationProperty(QIrisAttributeName)]
#endif
        public QIriConfigurationElementCollection QIris
        {
#if NETSTANDARD2_0
            get { return _qIris; }

            set { _qIris = value ?? new QIriConfigurationElementCollection(); }
#else
            get { return (QIriConfigurationElementCollection)this[QIrisAttributeName]; }

            internal set { this[QIrisAttributeName] = value; }
#endif
        }
    }
}