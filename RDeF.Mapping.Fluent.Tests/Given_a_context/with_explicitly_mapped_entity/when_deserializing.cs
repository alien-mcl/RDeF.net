using System.IO;
using System.Threading.Tasks;
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
        private IUnmappedProduct PrimaryEntity { get; set; }

        private IUnmappedProduct SecondaryEntity { get; set; }

        public override async Task TheTest()
        {
            PrimaryEntity = await Context.Load<IUnmappedProduct>(new Iri("temp:test"));
            SecondaryEntity = await Context.Load<IUnmappedProduct>(new Iri("another:test"));
        }

        [Test]
        public void Should_deserialize_explicitly_mapped_properties()
        {
            ((string)((dynamic)PrimaryEntity).TempName).Should().Be("Product name");
        }

        [Test]
        public void Should_deserialize_separate_explicitly_mapped_properties_for_each_entity()
        {
            ((string)((dynamic)SecondaryEntity).TempName).Should().Be("Another product name");
        }

        [Test]
        public void Should_deserialize_another_separate_explicitly_mapped_properties_for_each_entity()
        {
            ((string)((dynamic)SecondaryEntity).TempLabel).Should().Be("Alternative product name");
        }

        protected override void ScenarioSetup()
        {
            Context = new DefaultEntityContextFactory().Create();
            Context.UnmappedPropertyEncountered += OnUnmappedPropertyEncountered;
            ((ISerializableEntitySource)Context.EntitySource).Read(new StreamReader(typeof(when_deserializing).GetEmbeddedResource(".json")), new JsonLdReader());
        }

        private void OnUnmappedPropertyEncountered(object sender, UnmappedPropertyEventArgs e)
        {
            switch (e.Statement.Predicate.ToString())
            {
                case "temp:name":
                    e.OfEntity<IUnmappedProduct>(config => config
                        .WithProperty(typeof(string), "TempName")
                        .MappedTo(e.Statement.Predicate, e.Statement.Graph).WithDefaultConverter());
                    break;
                case "temp:label":
                    e.OfEntity<IUnmappedProduct>(config => config
                        .WithProperty(typeof(string), "TempLabel")
                        .MappedTo(e.Statement.Predicate, e.Statement.Graph).WithDefaultConverter());
                    break;
            }
        }
    }
}
