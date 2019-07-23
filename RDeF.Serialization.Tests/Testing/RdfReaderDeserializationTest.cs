using System.Collections.Generic;
using System.IO;
using RDeF.Entities;
using RDeF.Reflection;
using RDeF.Serialization;

namespace RDeF.Testing
{
    public abstract class RdfReaderDeserializationTest<T> : RdfReaderTest<T> where T : IRdfReader, new()
    {
        protected IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>> Result { get; set; }

        protected Stream Stream { get; private set; }

        protected IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>> Expected { get; private set; }

        public override void TheTest()
        {
            Result = Reader.Read(new StreamReader(Stream)).Result;
        }

        protected override void ScenarioSetup()
        {
            Stream = GetType().GetEmbeddedResource(
                EmbeddedResourceExtension,
                name => name.Replace("Reader_class", "Writer_class").Replace("deserializing", "serializing"));
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
