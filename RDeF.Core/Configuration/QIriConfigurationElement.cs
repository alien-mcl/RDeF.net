#if !NETSTANDARD2_0
using System.Configuration;
#endif
using System.Diagnostics.CodeAnalysis;

namespace RDeF.Configuration
{
    /// <summary>Describes a QIri mapping configuration.</summary>
    [ExcludeFromCodeCoverage]
    [SuppressMessage("TS0000", "NoUnitTests", Justification = "Simple configuration wrapper without special logic to be tested.")]
    public class QIriConfigurationElement
#if !NETSTANDARD2_0
        : ConfigurationElement
#endif
    {
#if !NETSTANDARD2_0
        private const string PrefixAttributeName = "prefix";
        private const string IriAttributeName = "iri";
#endif

        /// <summary>Gets a prefix of the QIri mapped.</summary>
#if !NETSTANDARD2_0
        [ConfigurationProperty(PrefixAttributeName, IsKey = true, IsRequired = true)]
#endif
        public string Prefix
        {
#if NETSTANDARD2_0
            get;

            set;
#else
            get { return (string)this[PrefixAttributeName]; }

            internal set { this[PrefixAttributeName] = value; }
#endif
        }

        /// <summary>Gets an Iri of the QIri mapped.</summary>
#if !NETSTANDARD2_0
        [ConfigurationProperty(IriAttributeName, IsRequired = true)]
#endif
        public string Iri
        {
#if NETSTANDARD2_0
            get;

            set;
#else
            get { return (string)this[IriAttributeName]; }

            internal set { this[IriAttributeName] = value; }
#endif
        }
    }
}