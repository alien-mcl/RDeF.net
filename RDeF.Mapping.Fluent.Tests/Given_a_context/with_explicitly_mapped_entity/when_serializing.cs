using System.IO;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using RDeF;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Mapping.Entities;
using RDeF.Reflection;
using RDeF.Serialization;

namespace Given_a_context.with_explicitly_mapped_entity
{
    [TestFixture]
    public class when_serializing : ExplicitMappingsTest
    {
        private static readonly string Expected = new StreamReader(typeof(when_serializing).GetEmbeddedResource(".json")).ReadToEnd().Cleaned();

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
            var primaryProduct = Context.Create<IUnmappedProduct>(new Iri("test"), MapPrimaryEntity);
            primaryProduct.Name = "Product name";
            primaryProduct.Description = "Product description";
            primaryProduct.Categories.Add("category 1");
            var secondaryProduct = Context.Create<IUnmappedProduct>(new Iri("another"), MapSecondaryEntity);
            secondaryProduct.Name = "Another product name";
            secondaryProduct.Description = "Product description";
            secondaryProduct.Categories.Add("category 1");
            Context.Commit();
            Buffer = new MemoryStream();
        }
    }
}
