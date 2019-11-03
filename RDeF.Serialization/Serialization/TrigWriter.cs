using VDS.RDF;

namespace RDeF.Serialization
{
    /// <summary>Provides an TriG serialization.</summary>
    public class TriGWriter : RdfWriterBase
    {
        /// <inheritdoc />
        protected override bool SupportsGraphs { get; } = true;

        /// <inheritdoc />
        protected override IStoreWriter CreateStoreWriter()
        {
            return new VDS.RDF.Writing.TriGWriter();
        }

        /// <inheritdoc />
        protected override VDS.RDF.IRdfWriter CreateRdfWriter()
        {
            return null;
        }
    }
}
