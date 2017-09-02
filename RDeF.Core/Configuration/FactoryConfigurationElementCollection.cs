#if NETSTANDARD2_0
using System.Collections.Generic;
#else
using System.Configuration;
#endif
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace RDeF.Configuration
{
    /// <summary>Acts as a collection of <see cref="FactoryConfigurationElement" />s.</summary>
    [ExcludeFromCodeCoverage]
    [SuppressMessage("TS0000", "NoUnitTests", Justification = "Simple configuration wrapper without special logic to be tested.")]
    [SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface", Justification = "Part of built in configuration API - manual usage is not recommended.")]
#if !NETSTANDARD2_0
    [ConfigurationCollection(typeof(FactoryConfigurationElement))]
#endif
    public class FactoryConfigurationElementCollection
#if NETSTANDARD2_0
        : List<FactoryConfigurationElement>
#else
        : ConfigurationElementCollection
#endif
    {
        /// <summary>Gets a named factory configuration.</summary>
        /// <param name="name">Name of the factory to obtain.</param>
        /// <returns>Instance of the <see cref="FactoryConfigurationElement" /> if a matching <paramref name="name" /> was found; otherwise <b>null</b>.</returns>
        public
#if !NETSTANDARD2_0
            new
#endif
            FactoryConfigurationElement this[string name]
        {
            get { return this.Cast<FactoryConfigurationElement>().FirstOrDefault(factory => factory.Name == name); }
        }

#if !NETSTANDARD2_0
        /// <inheritdoc />
        protected override ConfigurationElement CreateNewElement()
        {
            return new FactoryConfigurationElement();
        }

        /// <inheritdoc />
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FactoryConfigurationElement)element);
        }
#endif
    }
}