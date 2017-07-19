﻿#if NETSTANDARD1_6
using System.Collections.Generic;
#else
using System.Configuration;
#endif
using System.Diagnostics.CodeAnalysis;

namespace RDeF.Configuration
{
    /// <summary>Acts as a collection of <see cref="QIriConfigurationElement" />s.</summary>
    [ExcludeFromCodeCoverage]
    [SuppressMessage("TS0000", "NoUnitTests", Justification = "Simple configuration wrapper without special logic to be tested.")]
    [SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface", Justification = "Part of built in configuration API - manual usage is not recommended.")]
#if !NETSTANDARD1_6
    [ConfigurationCollection(typeof(QIriConfigurationElement))]
#endif
    public class QIriConfigurationElementCollection
#if NETSTANDARD1_6
        : List<QIriConfigurationElement>
#else
        : ConfigurationElementCollection
#endif
    {
#if !NETSTANDARD1_6
        /// <inheritdoc />
        protected override ConfigurationElement CreateNewElement()
        {
            return new QIriConfigurationElement();
        }

        /// <inheritdoc />
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((QIriConfigurationElement)element).Prefix;
        }
#endif
    }
}