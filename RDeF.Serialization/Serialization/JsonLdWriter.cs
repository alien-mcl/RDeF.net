using VDS.RDF;

namespace RDeF.Serialization
{
    /// <summary>Writes RDF data as a JSON-LD.</summary>
    public class JsonLdWriter : RdfWriterBase
    {
        /// <inheritdoc />
        protected override bool SupportsGraphs { get; } = true;

        /// <inheritdoc />
        protected override IStoreWriter CreateStoreWriter()
        {
            return new VDS.RDF.Writing.JsonLdWriter();
        }

        /// <inheritdoc />
        protected override VDS.RDF.IRdfWriter CreateRdfWriter()
        {
            return null;
        }
    }
}
