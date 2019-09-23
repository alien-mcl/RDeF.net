using System.IO;
using RDeF.Serialization;
using RDeF.Testing;

namespace Given_instance_of.JsonLdReader_class.when_deserializing
{
    public abstract class ScenarioTest : RdfReaderDeserializationTest<JsonLdReader>
    {
        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();
            new JsonLdWriter().Write(new StreamWriter(Stream), Expected).GetAwaiter().GetResult();
            Stream.Seek(0, SeekOrigin.Begin);
        }
    }
}
