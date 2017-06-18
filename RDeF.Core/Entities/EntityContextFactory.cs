﻿using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using RDeF.Configuration;

namespace RDeF.Entities
{
    /// <summary>Provides a factory method for creating configured instance of the <see cref="IEntityContextFactory" />.</summary>
    public static class EntityContextFactory
    {
        /// <summary>Creates a new <see cref="IEntityContextFactory" /> configured according to the <paramref name="configurationName" />.</summary>
        /// <param name="configurationName">Name of the configuration to use.</param>
        /// <returns>Instance of the <see cref="IEntityContextFactory" />.</returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "This is a factory method and object created is lost from the scope. Destructor is implemented which should clean up properly.")]
        public static IEntityContextFactory FromConfiguration(string configurationName)
        {
            var result = new DefaultEntityContextFactory();
            FactoryConfigurationElement factoryConfiguration;
            if ((RdefNetConfigurationSection.Default.Factories.TryGetValue(configurationName, out factoryConfiguration)) &&
                (factoryConfiguration.MappingAssemblies.Cast<MappingAssemblyConfigurationElement>().Any()))
            {
                result.WithMappings(
                    builder =>
                    {
                        foreach (MappingAssemblyConfigurationElement mappingAssemblyConfiguration in factoryConfiguration.MappingAssemblies)
                        {
                            var mappingAssembly = Assembly.Load(new AssemblyName(mappingAssemblyConfiguration.Name));
                            builder = builder.FromAssembly(mappingAssembly);
                        }
                    });
            }

            return result;
        }
    }
}
