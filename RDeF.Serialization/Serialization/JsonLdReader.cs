using System;
using Newtonsoft.Json.Linq;
using VDS.RDF;
using VDS.RDF.JsonLd;
using VDS.RDF.Parsing;

namespace RDeF.Serialization
{
    /// <summary>Allows to read JSON-LD graphs.</summary>
    public class JsonLdReader : RdfReaderBase
    {
        private readonly JToken _expandContext;
        private readonly Func<Uri, JsonLdDocument> _documentLoader;

        /// <summary>Initializes a new instance of the <see cref="JsonLdReader" /> class.</summary>
        public JsonLdReader() : this(null)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="JsonLdReader" /> class.</summary>
        /// <param name="expandContext">Additional expansion context.</param>
        /// <param name="documentLoader">Optional document loader.</param>
        public JsonLdReader(JToken expandContext, Func<Uri, JsonLdDocument> documentLoader = null)
        {
            _expandContext = expandContext;
            _documentLoader = documentLoader;
        }

        /// <inheritdoc />
        protected override bool SupportsGraphs { get; } = true;

        /// <inheritdoc />
        protected override IStoreReader CreateStoreReader(Uri baseUri)
        {
            var options = new JsonLdProcessorOptions() { Base = baseUri, ExpandContext = _expandContext };
            if (_documentLoader != null)
            {
                options.Loader = _ => JsonLdDocument.ToRemoteDocument(_documentLoader(_));
            }

            return new JsonLdParser(options);
        }

        /// <inheritdoc />
        protected override VDS.RDF.IRdfReader CreateRdfReader(Uri baseUri)
        {
            return null;
        }
    }
}
