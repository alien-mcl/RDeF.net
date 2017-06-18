using System.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace RDeF.Configuration
{
    /// <summary>Acts as a collection of <see cref="FactoryConfigurationElement" />s.</summary>
    [ExcludeFromCodeCoverage]
    [SuppressMessage("TS0000", "NoUnitTests", Justification = "Simple configuration wrapper without special logic to be tested.")]
    [SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface", Justification = "Part of built in configuration API - manual usage is not recommended.")]
    [ConfigurationCollection(typeof(FactoryConfigurationElement))]
    public class FactoryConfigurationElementCollection : ConfigurationElementCollection
    {
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
    }
}