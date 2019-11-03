using NUnit.Framework;
using RDeF.Serialization;
using RDeF.Testing;

namespace Given_instance_of.writer_of_type
{
    [TestFixture]
    public class RdfXmlWriter_class : RdfWriterTest<RdfXmlWriter, RdfXmlReader>
    {
        protected override bool UseStrictGraphMatching { get; } = false;
    }
}
