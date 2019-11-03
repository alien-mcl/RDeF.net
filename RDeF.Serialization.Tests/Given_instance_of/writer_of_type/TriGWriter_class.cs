using NUnit.Framework;
using RDeF.Serialization;
using RDeF.Testing;

namespace Given_instance_of.writer_of_type
{
    [TestFixture]
    public class TriGWriter_class : RdfWriterTest<TriGWriter, TriGReader>
    {
        protected override bool UseStrictGraphMatching { get; } = true;
    }
}
