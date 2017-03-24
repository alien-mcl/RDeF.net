using System.Collections.Generic;
using System.IO;
using RDeF;
using RDeF.Entities;

namespace Given_instance_of.JsonLdWriter_class.when_serializing
{
    public abstract class ScenarioTest : JsonLdWriterTest
    {
        protected string Expected { get; private set; }

        protected MemoryStream Stream { get; private set; }

        protected IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>> Graphs { get; set; }

        private string ExpectedSerializationResourceName { get; set; }

        public override void TheTest()
        {
            Writer.Write(new StreamWriter(Stream), Graphs);
        }

        protected override void ScenarioSetup()
        {
            ExpectedSerializationResourceName = GetType().FullName.Replace(".", "\\") + ".json";
            Expected = new StreamReader(typeof(JsonLdWriterTest).Assembly.GetManifestResourceStream(ExpectedSerializationResourceName)).ReadToEnd().Cleaned();
            Stream = new MemoryStream();
        }
    }
}
