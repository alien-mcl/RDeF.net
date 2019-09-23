using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using RDeF.Serialization;

namespace RDeF.Testing
{
    public abstract class RdfWriterSerializationTest<T> : RdfWriterTest<T> where T : IRdfWriter, new()
    {
        protected MemoryStream Stream { get; private set; }

        protected IEnumerable<IGraph> Graphs { get; set; }

        public override async Task TheTest()
        {
            await Writer.Write(new StreamWriter(Stream), Graphs);
            Stream.Seek(0, SeekOrigin.Begin);
        }

        protected override void ScenarioSetup()
        {
            Stream = new MemoryStream();
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
