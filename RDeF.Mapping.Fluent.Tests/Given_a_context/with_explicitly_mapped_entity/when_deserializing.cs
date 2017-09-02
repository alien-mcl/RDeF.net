using System.IO;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Reflection;
using RDeF.Serialization;

namespace Given_a_context.with_explicitly_mapped_entity
{
    [TestFixture]
    public class when_deserializing : ExplicitMappingsTest
    {
        private IUnmappedProduct Result { get; set; }

        public override void TheTest()
        {
            Result = Context.Load<IUnmappedProduct>(new Iri("temp:test"));
        }

        [Test]
        public void Should_serialize_explicitly_mapped_properties()
        {
            ((string)((dynamic)Result).Test).Should().Be("Product name");
        }

        protected override void ScenarioSetup()
        {
            Context = new DefaultEntityContextFactory().Create();
            Context.UnmappedPropertyEncountered += OnUnmappedPropertyEncountered;
            ((ISerializableEntitySource)Context.EntitySource).Read(new StreamReader(typeof(when_deserializing).GetEmbeddedResource(".json")), new JsonLdReader());
        }

        private void OnUnmappedPropertyEncountered(object sender, UnmappedPropertyEventArgs e)
        {
            if (e.Statement.Predicate == "temp:name")
            {
                e.OfEntity<IUnmappedProduct>(config => config
                    .WithProperty(typeof(string), "Test").MappedTo(e.Statement.Predicate, e.Statement.Graph).WithDefaultConverter());
            }
        }
    }
}
