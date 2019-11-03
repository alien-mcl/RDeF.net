using NUnit.Framework;
using RDeF.Serialization;
using RDeF.Testing;

namespace Given_instance_of.writer_of_type
{
    [TestFixture]
    public class N3Writer_class : RdfWriterTest<N3Writer, N3Reader>
    {
        protected override bool UseStrictGraphMatching { get; } = false;
    }
}
