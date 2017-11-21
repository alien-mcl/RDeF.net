using System.IO;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
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
        private static readonly JObject Expected =
            (JObject)JsonConvert.DeserializeObject(new StreamReader(typeof(when_serializing).GetEmbeddedResource(".json")).ReadToEnd());

        private MemoryStream Buffer { get; set; }

        private JObject Result { get; set; }

        private JObject ResultConverted
        {
            get { return Result ?? (Result = (JObject)JsonConvert.DeserializeObject(Encoding.UTF8.GetString(Buffer.ToArray()))); }
        }

        public override void TheTest()
        {
            ((ISerializableEntitySource)Context.EntitySource).Write(new StreamWriter(Buffer), new JsonLdWriter());
        }

        [Test]
        public void Should_serialize_explicitly_mapped_properties()
        {
            ResultConverted.ShouldBeEquivalentTo(Expected);
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
            secondaryProduct = Context.Load<IUnmappedProduct>(new Iri("another"), AlternativeMapSecondaryEntity);
            secondaryProduct.Name = "Alternative product name";
            Context.Commit();
            Buffer = new MemoryStream();
        }
    }
}
