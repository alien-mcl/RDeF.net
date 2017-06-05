using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using RDeF.Entities;
using RDeF.Serialization;

namespace RDeF.Testing
{
    public abstract class RdfReaderDeserializationTest<T> : RdfReaderTest<T> where T : IRdfReader, new()
    {
        protected IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>> Result { get; set; }

        protected Stream Stream { get; private set; }

        protected IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>> Expected { get; private set; }

        private string ExpectedDeserializationResourceName { get; set; }

        public override void TheTest()
        {
            Result = Reader.Read(new StreamReader(Stream)).Result;
        }

        protected override void ScenarioSetup()
        {
            ExpectedDeserializationResourceName = GetType().FullName.Replace("Reader_class", "Writer_class").Replace("deserializing", "serializing").Replace(".", "\\");
            var extension = Regex.Match(ExpectedDeserializationResourceName, "\\\\([A-Z][a-z]+)").Groups[1].Value.ToLower();
            ExpectedDeserializationResourceName += "." + extension;
            Stream = GetType().GetTypeInfo().Assembly.GetManifestResourceStream(ExpectedDeserializationResourceName);
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
