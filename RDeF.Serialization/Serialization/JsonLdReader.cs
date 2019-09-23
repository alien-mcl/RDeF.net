using System;
using VDS.RDF;
using VDS.RDF.JsonLd;
using VDS.RDF.Parsing;

namespace RDeF.Serialization
{
    /// <summary>Allows to read JSON-LD graphs.</summary>
    public class JsonLdReader : RdfReaderBase
    {
        /// <inheritdoc />
        protected override bool SupportsGraphs { get; } = true;

        /// <inheritdoc />
        protected override IStoreReader CreateStoreReader(Uri baseUri)
        {
            return new JsonLdParser(new JsonLdProcessorOptions() { Base = baseUri });
        }

        /// <inheritdoc />
        protected override VDS.RDF.IRdfReader CreateRdfReader(Uri baseUri)
        {
            return null;
        }
    }
}
