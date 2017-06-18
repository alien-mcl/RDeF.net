using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using RDeF.Entities;

namespace RDeF.Configuration
{
    /// <summary>Describes an <see cref="IEntityContextFactory" /> configuration.</summary>
    [ExcludeFromCodeCoverage]
    [SuppressMessage("TS0000", "NoUnitTests", Justification = "Simple configuration wrapper without special logic to be tested.")]
    public class FactoryConfigurationElement : ConfigurationElement
    {
        private const string NameAttributeName = "name";
        private const string MappingAssembliesAttributeName = "mappingAssemblies";

        /// <summary>Gets a name of the factory configuration.</summary>
        [ConfigurationProperty(NameAttributeName, IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this[NameAttributeName]; }

            internal set { this[NameAttributeName] = value; }
        }

        /// <summary>Gets a collection of mapping assemblies.</summary>
        [ConfigurationProperty(MappingAssembliesAttributeName)]
        public MappingAssemblyConfigurationElementCollection MappingAssemblies
        {
            get { return (MappingAssemblyConfigurationElementCollection)this[MappingAssembliesAttributeName]; }

            internal set { this[MappingAssembliesAttributeName] = value; }
        }
    }
}