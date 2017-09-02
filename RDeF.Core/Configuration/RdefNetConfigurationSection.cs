#if NETSTANDARD2_0
using System.Collections.Concurrent;
using System.Collections.Generic;
#else
using System.Configuration;
#endif
using System.Diagnostics.CodeAnalysis;
#if NETSTANDARD2_0
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
#endif

namespace RDeF.Configuration
{
    /// <summary>Describes an RDeF.net configuration section.</summary>
    [ExcludeFromCodeCoverage]
    [SuppressMessage("TS0000", "NoUnitTests", Justification = "Simple configuration wrapper without special logic to be tested.")]
    public class RdefNetConfigurationSection
#if !NETSTANDARD2_0
        : ConfigurationSection
#endif
    {
        /// <summary>Defines a default name of the RDeF.net configuration section.</summary>
        public const string DefaultConfigurationSectionName = "rdef.net";
#if NETSTANDARD2_0
        private const string ConfigurationFileName = "appsettings.json";
        private static readonly IConfigurationRoot Configuration;
        private static IDictionary<string, RdefNetConfigurationSection> _configurations = new ConcurrentDictionary<string, RdefNetConfigurationSection>();
        private FactoryConfigurationElementCollection _factories = new FactoryConfigurationElementCollection();

        static RdefNetConfigurationSection()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(DiscoverConfigurationFile(), true);
            Configuration = configurationBuilder.Build();
        }
#else
        private const string FactoriesAttributeName = "factories";
#endif

        /// <summary>Gets a default configuration.</summary>
        public static RdefNetConfigurationSection Default
        {
            get { return GetConfiguration(DefaultConfigurationSectionName); }
        }

        /// <summary>Gets a map of named entity context factory configurations.</summary>
#if !NETSTANDARD2_0
        [ConfigurationProperty(FactoriesAttributeName)]
#endif
        public FactoryConfigurationElementCollection Factories
        {
#if NETSTANDARD2_0
            get { return _factories; }

            internal set { _factories = value ?? new FactoryConfigurationElementCollection(); }
#else
            get { return (FactoryConfigurationElementCollection)this[FactoriesAttributeName]; }

            internal set { this[FactoriesAttributeName] = value; }
#endif
        }

        private static RdefNetConfigurationSection GetConfiguration(string name)
        {
#if NETSTANDARD2_0
            RdefNetConfigurationSection result;
            if (_configurations.TryGetValue(name, out result))
            {
                return result;
            }

            _configurations[name] = result = new RdefNetConfigurationSection();
            var configurationBinder = new ConfigureFromConfigurationOptions<RdefNetConfigurationSection>(Configuration.GetSection(name));
            try
            {
                configurationBinder.Configure(result);
            }
            catch (TargetInvocationException error)
            {
                throw error.InnerException;
            }

            return result;
#else
            return (RdefNetConfigurationSection)ConfigurationManager.GetSection(name) ??
                   new RdefNetConfigurationSection()
                   {
                       Factories = new FactoryConfigurationElementCollection()
                   };
#endif
        }

#if NETSTANDARD2_0
        private static string DiscoverConfigurationFile()
        {
            var path = Directory.GetCurrentDirectory();
            if (!File.Exists(Path.Combine(path, ConfigurationFileName)))
            {
                return (from directory in Directory.GetDirectories(path)
                        from file in Directory.GetFiles(directory, ConfigurationFileName)
                        select file).FirstOrDefault() ?? Path.Combine(path, ConfigurationFileName);
            }

            return Path.Combine(path, ConfigurationFileName);
        }
#endif
    }
}