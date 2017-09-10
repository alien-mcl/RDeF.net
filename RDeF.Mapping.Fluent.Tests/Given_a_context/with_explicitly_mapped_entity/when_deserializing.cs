using System;
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
        private IUnmappedProduct PrimaryEntity { get; set; }

        private IUnmappedProduct SecondaryEntity { get; set; }

        private int MappingCalls { get; set; }

        public override void TheTest()
        {
            PrimaryEntity = Context.Load<IUnmappedProduct>(new Iri("temp:test"));
            SecondaryEntity = Context.Load<IUnmappedProduct>(new Iri("another:test"));
        }

        [Test]
        public void Should_deserialize_explicitly_mapped_properties()
        {
            ((string)((dynamic)PrimaryEntity).Test).Should().Be("Product name");
        }

        [Test]
        public void Should_deserialize_separate_explicitly_mapped_properties_for_each_entity()
        {
            ((string)((dynamic)SecondaryEntity).Test2).Should().Be("Another product name");
        }

        protected override void ScenarioSetup()
        {
            MappingCalls = 1;
            Context = new DefaultEntityContextFactory().Create();
            Context.UnmappedPropertyEncountered += OnUnmappedPropertyEncountered;
            ((ISerializableEntitySource)Context.EntitySource).Read(new StreamReader(typeof(when_deserializing).GetEmbeddedResource(".json")), new JsonLdReader());
        }

        private void OnUnmappedPropertyEncountered(object sender, UnmappedPropertyEventArgs e)
        {
            if (e.Statement.Predicate == "temp:name")
            {
                e.OfEntity<IUnmappedProduct>(config => config
                    .WithProperty(typeof(string), "Test" + (MappingCalls == 1 ? String.Empty : MappingCalls.ToString()))
                        .MappedTo(e.Statement.Predicate, e.Statement.Graph).WithDefaultConverter());
                MappingCalls++;
            }
        }
    }
}
