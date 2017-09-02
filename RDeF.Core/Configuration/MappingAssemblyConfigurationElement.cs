#if !NETSTANDARD2_0
using System.Configuration;
#endif
using System.Diagnostics.CodeAnalysis;

namespace RDeF.Configuration
{
    /// <summary>Describes a mapping assembly configuration.</summary>
    [ExcludeFromCodeCoverage]
    [SuppressMessage("TS0000", "NoUnitTests", Justification = "Simple configuration wrapper without special logic to be tested.")]
    public class MappingAssemblyConfigurationElement
#if !NETSTANDARD2_0
        : ConfigurationElement
#endif
    {
#if !NETSTANDARD2_0
        private const string NameAttributeName = "name";
#endif

        /// <summary>Gets a name of the assembly to be used for mappings.</summary>
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
    }
}