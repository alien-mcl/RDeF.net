using System;
using VDS.RDF;
using VDS.RDF.Parsing;

namespace RDeF.Serialization
{
    /// <summary>Allows to read N3 graphs.</summary>
    public class N3Reader : RdfReaderBase
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
            return new Notation3Parser();
        }
    }
}
