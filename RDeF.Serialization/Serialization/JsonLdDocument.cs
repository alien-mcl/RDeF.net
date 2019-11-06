using System;
using Newtonsoft.Json.Linq;
using VDS.RDF.JsonLd;

namespace RDeF.Serialization
{
    /// <summary>Describes a remote document.</summary>
    public class JsonLdDocument
    {
        /// <summary>Gets or sets an optional JSON-LD context uri obtained from <i>Link</i> header.</summary>
        public Uri ContextUri { get; set; }

        /// <summary>Gets or sets a final document uri.</summary>
        public Uri DocumentUri { get; set; }

        /// <summary>Gets or sets the document.</summary>
        public JToken Document { get; set; }

        internal static RemoteDocument ToRemoteDocument(JsonLdDocument document)
        {
            RemoteDocument result = null;
            if (document?.Document != null && document.DocumentUri != null)
            {
                result = new RemoteDocument()
                {
                    ContextUrl = document.ContextUri,
                    DocumentUrl = document.DocumentUri,
                    Document = document.Document
                };
            }

            return result;
        }
    }
}
