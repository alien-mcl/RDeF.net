using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace RDeF.Configuration
{
    /// <summary>Describes an RDeF.net configuration section.</summary>
    [ExcludeFromCodeCoverage]
    [SuppressMessage("TS0000", "NoUnitTests", Justification = "Simple configuration wrapper without special logic to be tested.")]
    public class RdefNetConfigurationSection : ConfigurationSection
    {
        /// <summary>Defines a default name of the RDeF.net configuration section.</summary>
        public const string DefaultConfigurationSectionName = "rdef.net";
        private const string FactoriesAttributeName = "factories";

        /// <summary>Gets a default configuration.</summary>
        public static RdefNetConfigurationSection Default
        {
            get
            {
                return (RdefNetConfigurationSection)ConfigurationManager.GetSection(DefaultConfigurationSectionName) ??
                    new RdefNetConfigurationSection()
                    {
                        AllFactories = new FactoryConfigurationElementCollection()
                    };
            }
        }

        /// <summary>Gets a map of named entity context factory configurations.</summary>
        public IDictionary<string, FactoryConfigurationElement> Factories
        {
            get { return AllFactories.Cast<FactoryConfigurationElement>().ToDictionary(factory => factory.Name, factory => factory); }
        }

        [ConfigurationProperty(FactoriesAttributeName)]
        internal FactoryConfigurationElementCollection AllFactories
        {
            get { return (FactoryConfigurationElementCollection)this[FactoriesAttributeName]; }

            set { this[FactoriesAttributeName] = value; }
        }
    }
}
