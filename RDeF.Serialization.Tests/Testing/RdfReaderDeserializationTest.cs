using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using RDeF.Serialization;

namespace RDeF.Testing
{
    public abstract class RdfReaderDeserializationTest<T> : RdfReaderTest<T> where T : IRdfReader, new()
    {
        protected IEnumerable<IGraph> Result { get; set; }

        protected Stream Stream { get; private set; }

        protected IEnumerable<IGraph> Expected { get; private set; }

        public override async Task TheTest()
        {
            Result = await Reader.Read(new StreamReader(Stream));
        }

        protected override void ScenarioSetup()
        {
            Stream = new MemoryStream();
        }

        protected void WithSimpleGraph()
        {
            Expected = RdfTestSets.SimpleGraph;
        }

        protected void WithCollectionsGraph()
        {
            Expected = RdfTestSets.ComplexGraph;
        }
    }
}
