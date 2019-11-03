using NUnit.Framework;
using RDeF.Serialization;
using RDeF.Testing;

namespace Given_instance_of.reader_of_type
{
    [TestFixture]
    public class TriGReader_class : RdfReaderTest<TriGReader, TriGWriter>
    {
        protected override bool UseStrictGraphMatching { get; } = true;
    }
}
