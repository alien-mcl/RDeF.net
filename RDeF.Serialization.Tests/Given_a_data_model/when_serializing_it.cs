using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RDeF.FluentAssertions;
using RDeF.Reflection;
using RDeF.Serialization;

namespace Given_a_data_model
{
    [TestFixture]
    public class when_serializing_it
    {
        private static readonly IEnumerable<IGraph> Expected =
            new JsonLdReader().Read(new StreamReader(typeof(when_serializing_it).GetEmbeddedResource(".json"))).Result;

        private MemoryStream Buffer { get; set; }

        private IEntityContext Context { get; set; }

        private IRdfWriter RdfWriter { get; set; }

        public void TheTest()
        {
            using (var streamWriter = new StreamWriter(Buffer, Encoding.UTF8, 1024, true))
            {
                (Context.EntitySource as ISerializableEntitySource).Write(streamWriter, RdfWriter);
            }

            Buffer.Seek(0, SeekOrigin.Begin);
        }

        [Test]
        public async Task Should_create_a_correct_JsonLd()
        {
            (await new JsonLdReader().Read(new StreamReader(Buffer))).Should().BeSimilarTo(Expected);
        }

        [SetUp]
        public void Setup()
        {
            Buffer = new MemoryStream();
            RdfWriter = new JsonLdWriter();
            ScenarioSetup();
            TheTest();
        }

        protected virtual void ScenarioSetup()
        {
            Context = new DefaultEntityContextFactory()
                .WithMappings(_ => _.FromAssemblyOf<IComplexEntity>())
                .Create();
            var entity = Context.Create<IComplexEntity>(new Iri("http://test.uri/entity"));
            entity.Floats.Add(0.1f);
            entity.Floats.Add(0.2f);
            entity.Doubles.Add(1.0);
            entity.Doubles.Add(2.0);
            entity.Related.Add(entity);
            entity.Other.Add(entity);
            Context.Commit();
        }
    }
}
