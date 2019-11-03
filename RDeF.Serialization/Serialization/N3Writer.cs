using VDS.RDF;

namespace RDeF.Serialization
{
    /// <summary>Provides an N3 serialization.</summary>
    public class N3Writer : RdfWriterBase
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
            return new VDS.RDF.Writing.Notation3Writer();
        }
    }
}
