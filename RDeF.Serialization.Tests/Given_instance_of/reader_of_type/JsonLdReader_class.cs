using NUnit.Framework;
using RDeF.Serialization;
using RDeF.Testing;

namespace Given_instance_of.reader_of_type
{
    [TestFixture]
    public class JsonLdReader_class : RdfReaderTest<JsonLdReader, JsonLdWriter>
    {
        protected override bool UseStrictGraphMatching { get; } = true;
    }
}
