#if !NETSTANDARD1_6
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
#if !NETSTANDARD1_6
        : ConfigurationElement
#endif
    {
#if !NETSTANDARD1_6
        private const string NameAttributeName = "name";
        private const string MappingAssembliesAttributeName = "mappingAssemblies";
        private const string QIrisAttributeName = "qiris";
#endif

        /// <summary>Gets a name of the factory configuration.</summary>
#if !NETSTANDARD1_6
        [ConfigurationProperty(NameAttributeName, IsKey = true, IsRequired = true)]
#endif
        public string Name
        {
#if NETSTANDARD1_6
            get;

            set;
#else
            get { return (string)this[NameAttributeName]; }

            internal set { this[NameAttributeName] = value; }
#endif
        }

        /// <summary>Gets a collection of mapping assemblies.</summary>
#if !NETSTANDARD1_6
        [ConfigurationProperty(MappingAssembliesAttributeName)]
#endif
        public MappingAssemblyConfigurationElementCollection MappingAssemblies
        {
#if NETSTANDARD1_6
            get;

            set;
#else
            get { return (MappingAssemblyConfigurationElementCollection)this[MappingAssembliesAttributeName]; }

            internal set { this[MappingAssembliesAttributeName] = value; }
#endif
        }

        /// <summary>Gets a QIri mappings.</summary>
#if !NETSTANDARD1_6
        [ConfigurationProperty(QIrisAttributeName)]
#endif
        public QIriConfigurationElementCollection QIris
        {
#if NETSTANDARD1_6
            get;

            set;
#else
            get { return (QIriConfigurationElementCollection)this[QIrisAttributeName]; }

            internal set { this[QIrisAttributeName] = value; }
#endif
        }
    }
}