using System.IO;
using RDeF.Serialization;
using RDeF.Testing;

namespace Given_instance_of.RdfXmlReader_class.when_deserializing
{
    public abstract class ScenarioTest : RdfReaderDeserializationTest<RdfXmlReader>
    {
        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();
            new RdfXmlWriter().Write(new StreamWriter(Stream), Expected).GetAwaiter().GetResult();
            Stream.Seek(0, SeekOrigin.Begin);
        }
    }
}
