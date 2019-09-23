using VDS.RDF;

namespace RDeF.Serialization
{
    /// <summary>Provides a simple RDF over XML serialization.</summary>
    public class RdfXmlWriter : RdfWriterBase
    {
        /// <inheritdoc />
        protected override bool SupportsGraphs { get; } = false;

        /// <inheritdoc />
        protected override IStoreWriter CreateStoreWriter()
        {
            return null;
        }

        /// <inheritdoc />
        protected override VDS.RDF.IRdfWriter CreateRdfWriter()
        {
            return new VDS.RDF.Writing.RdfXmlWriter();
        }
    }
}
