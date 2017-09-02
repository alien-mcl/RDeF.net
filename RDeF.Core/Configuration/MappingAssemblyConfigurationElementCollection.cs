﻿#if NETSTANDARD2_0
using System.Collections.Generic;
#else
using System.Configuration;
#endif
using System.Diagnostics.CodeAnalysis;

namespace RDeF.Configuration
{
    /// <summary>Acts as a collection of <see cref="MappingAssemblyConfigurationElement" />s.</summary>
    [ExcludeFromCodeCoverage]
    [SuppressMessage("TS0000", "NoUnitTests", Justification = "Simple configuration wrapper without special logic to be tested.")]
    [SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface", Justification = "Part of built in configuration API - manual usage is not recommended.")]
#if !NETSTANDARD2_0
    [ConfigurationCollection(typeof(MappingAssemblyConfigurationElement))]
#endif
    public class MappingAssemblyConfigurationElementCollection
#if NETSTANDARD2_0
        : List<MappingAssemblyConfigurationElement>
#else
        : ConfigurationElementCollection
#endif
    {
#if !NETSTANDARD2_0
        /// <inheritdoc />
        protected override ConfigurationElement CreateNewElement()
        {
            return new MappingAssemblyConfigurationElement();
        }

        /// <inheritdoc />
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MappingAssemblyConfigurationElement)element).Name;
        }
#endif
    }
}