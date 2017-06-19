using System.Collections.Generic;
using System.IO;
using RDeF.Entities;
using RDeF.Reflection;
using RDeF.Serialization;

namespace RDeF.Testing
{
    public abstract class RdfWriterSerializationTest<T> : RdfWriterTest<T> where T : IRdfWriter, new()
    {
        protected string Expected { get; private set; }

        protected MemoryStream Stream { get; private set; }

        protected IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>> Graphs { get; set; }

        public override void TheTest()
        {
            Writer.Write(new StreamWriter(Stream), Graphs).GetAwaiter().GetResult();
        }

        protected override void ScenarioSetup()
        {
            Stream = new MemoryStream();
            Expected = new StreamReader(GetType().GetEmbeddedResource(EmbeddedResourceExtension)).ReadToEnd();
            if (EmbeddedResourceExtension != "rdf")
            {
                Expected = Expected.Cleaned();
            }
        }

        protected void WithSimpleGraph()
        {
            Graphs = RdfTestSets.SimpleGraph;
        }

        protected void WithCollectionsGraph()
        {
            Graphs = RdfTestSets.ComplexGraph;
        }
    }
}
