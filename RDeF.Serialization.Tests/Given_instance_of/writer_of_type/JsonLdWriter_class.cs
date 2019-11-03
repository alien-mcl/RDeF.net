using NUnit.Framework;
using RDeF.Serialization;
using RDeF.Testing;

namespace Given_instance_of.writer_of_type
{
    [TestFixture]
    public class JsonLdWriter_class : RdfWriterTest<JsonLdWriter, JsonLdReader>
    {
        protected override bool UseStrictGraphMatching { get; } = true;
    }
}
