using NUnit.Framework;
using RDeF.Serialization;
using RDeF.Testing;

namespace Given_instance_of.reader_of_type
{
    [TestFixture]
    public class RdfXmlReader_class : RdfReaderTest<RdfXmlReader, RdfXmlWriter>
    {
        protected override bool UseStrictGraphMatching { get; } = false;
    }
}
