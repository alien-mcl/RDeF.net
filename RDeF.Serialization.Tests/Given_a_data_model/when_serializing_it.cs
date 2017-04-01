using System.IO;
using System.Reflection;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using RDeF;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Serialization;

namespace Given_a_data_model
{
    [TestFixture]
    public class when_serializing_it
    {
        private static readonly string ExpectedSerializationResourceName = typeof(when_serializing_it).FullName.Replace(".", "\\") + ".json";
        private static readonly string Expected = new StreamReader(typeof(when_serializing_it).GetTypeInfo().Assembly.GetManifestResourceStream(ExpectedSerializationResourceName)).ReadToEnd().Cleaned();

        private MemoryStream Buffer { get; set; }

        private IEntityContext Context { get; set; }

        private IRdfWriter RdfWriter { get; set; }

        public void TheTest()
        {
            using (var streamWriter = new StreamWriter(Buffer))
            {
                (Context.EntitySource as ISerializableEntitySource).Write(streamWriter, RdfWriter);
            }
        }

        [Test]
        public void Should_create_a_correct_JsonLd()
        {
            Encoding.UTF8.GetString(Buffer.ToArray()).Cleaned().Should().Be(Expected);
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
