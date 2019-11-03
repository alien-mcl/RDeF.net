using System;
using VDS.RDF;
using VDS.RDF.Parsing;

namespace RDeF.Serialization
{
    /// <summary>Allows to read TriG graphs.</summary>
    public class TriGReader : RdfReaderBase
    {
        /// <inheritdoc />
        protected override bool SupportsGraphs { get; } = true;

        /// <inheritdoc />
        protected override IStoreReader CreateStoreReader(Uri baseUri)
        {
            return new TriGParser();
        }

        /// <inheritdoc />
        protected override VDS.RDF.IRdfReader CreateRdfReader(Uri baseUri)
        {
            return null;
        }
    }
}
