using System;
using VDS.RDF;
using VDS.RDF.Parsing;

namespace RDeF.Serialization
{
    /// <summary>Deserializies RDF/XML into a graph aligned statements.</summary>
    public class RdfXmlReader : RdfReaderBase
    {
        /// <inheritdoc />
        protected override bool SupportsGraphs { get; } = false;

        /// <inheritdoc />
        protected override IStoreReader CreateStoreReader(Uri baseUri)
        {
            return null;
        }

        /// <inheritdoc />
        protected override VDS.RDF.IRdfReader CreateRdfReader(Uri baseUri)
        {
            return new RdfXmlParser();
        }
    }
}
