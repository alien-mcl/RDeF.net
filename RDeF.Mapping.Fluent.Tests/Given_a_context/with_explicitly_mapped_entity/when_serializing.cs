using System.IO;
using System.Reflection;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using RDeF;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Mapping.Entities;
using RDeF.Serialization;

namespace Given_a_context.with_explicitly_mapped_entity
{
    [TestFixture]
    public class when_serializing : ExplicitMappingsTest
    {
        private static readonly string Expected = new StreamReader(typeof(when_serializing).GetTypeInfo().Assembly
            .GetManifestResourceStream(typeof(when_serializing).FullName.Replace(".", "\\") + ".json")).ReadToEnd().Cleaned();

        private MemoryStream Buffer { get; set; }

        public override void TheTest()
        {
            ((ISerializableEntitySource)Context.EntitySource).Write(new StreamWriter(Buffer), new JsonLdWriter());
        }

        [Test]
        public void Should_serialize_explicitly_mapped_properties()
        {
            Encoding.UTF8.GetString(Buffer.ToArray()).Cleaned().Should().Be(Expected);
        }

        protected override void ScenarioSetup()
        {
            Context = new DefaultEntityContextFactory().Create();
            var product = Context.Create<IUnmappedProduct>(new Iri("test"), MapEntity);
            product.Name = "Product name";
            product.Description = "Product description";
            product.Categories.Add("category 1");
            Context.Commit();
            Buffer = new MemoryStream();
        }
    }
}
