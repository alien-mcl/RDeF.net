using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using RDeF.Entities;
using RDeF.Serialization;

namespace RDeF.Testing
{
    public abstract class RdfWriterSerializationTest<T> : RdfWriterTest<T> where T : IRdfWriter, new()
    {
        protected string Expected { get; private set; }

        protected MemoryStream Stream { get; private set; }

        protected IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>> Graphs { get; set; }

        private string ExpectedSerializationResourceName { get; set; }

        public override void TheTest()
        {
            Writer.Write(new StreamWriter(Stream), Graphs).GetAwaiter().GetResult();
        }

        protected override void ScenarioSetup()
        {
            ExpectedSerializationResourceName = GetType().FullName.Replace(".", "\\");
            var extension = Regex.Match(ExpectedSerializationResourceName, "\\\\([A-Z][a-z]+)").Groups[1].Value.ToLower();
            ExpectedSerializationResourceName += "." + extension;
            Expected = new StreamReader(GetType().GetTypeInfo().Assembly.GetManifestResourceStream(ExpectedSerializationResourceName)).ReadToEnd();
            if (extension != "rdf")
            {
                Expected = Expected.Cleaned();
            }

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
