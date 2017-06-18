using System.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace RDeF.Configuration
{
    /// <summary>Describes a mapping assembly configuration.</summary>
    [ExcludeFromCodeCoverage]
    [SuppressMessage("TS0000", "NoUnitTests", Justification = "Simple configuration wrapper without special logic to be tested.")]
    public class MappingAssemblyConfigurationElement : ConfigurationElement
    {
        private const string NameAttributeName = "name";

        /// <summary>Gets a nem of the assembly to be used for mappings.</summary>
        [ConfigurationProperty(NameAttributeName, IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this[NameAttributeName]; }

            internal set { this[NameAttributeName] = value; }
        }
    }
}