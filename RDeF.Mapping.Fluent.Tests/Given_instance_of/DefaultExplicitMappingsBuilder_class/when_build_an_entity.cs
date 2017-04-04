using System;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Entities;

namespace Given_instance_of.DefaultExplicitMappingsBuilder_class
{
    [TestFixture]
    public class when_build_an_entity : DefaultExplicitMappingsBuilderTest
    {
        public override void TheTest()
        {
            Builder.MappedTo(new Iri("term"), new Iri("graph"));
        }

        [Test]
        public void Should_throw_when_no_mapped_term_is_given()
        {
            Builder.Invoking(instance => instance.MappedTo(null)).ShouldThrow<ArgumentNullException>();
        }
        
        [Test]
        public void Should_prepare_entity_data_model_correctly()
        {
            Builder.Classes.Should().Contain(new Tuple<Iri, Iri>(new Iri("term"), new Iri("graph")));
        }
    }
}
