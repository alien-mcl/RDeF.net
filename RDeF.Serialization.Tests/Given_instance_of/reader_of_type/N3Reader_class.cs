using NUnit.Framework;
using RDeF.Serialization;
using RDeF.Testing;

namespace Given_instance_of.reader_of_type
{
    [TestFixture]
    public class N3Reader_class : RdfReaderTest<N3Reader, N3Writer>
    {
        protected override bool UseStrictGraphMatching { get; } = false;
    }
}
