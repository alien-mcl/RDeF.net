using System;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Entities;

namespace Given_instance_of.DefaultExplicitMappingsBuilder_class
{
    [TestFixture]
    public class when_building_an_entity : DefaultExplicitMappingsBuilderTest
    {
        public override void TheTest()
        {
            Builder.MappedTo(new Iri("term"), new Iri("graph"));
        }

        [Test]
        public void Should_throw_when_no_mapped_term_is_given()
        {
            Builder.Invoking(instance => instance.MappedTo(null))
                .Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Should_throw_when_no_prefix_is_given()
        {
            Builder.Invoking(instance => instance.MappedTo((string)null, null))
                .Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("prefix");
        }

        [Test]
        public void Should_throw_when_a_given_prefix_is_empty()
        {
            Builder.Invoking(instance => instance.MappedTo(String.Empty, null))
                .Should().Throw<ArgumentOutOfRangeException>().Which.ParamName.Should().Be("prefix");
        }

        [Test]
        public void Should_throw_when_no_term_is_given()
        {
            Builder.Invoking(instance => instance.MappedTo("test", null))
                .Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("term");
        }

        [Test]
        public void Should_throw_when_a_given_term_is_empty()
        {
            Builder.Invoking(instance => instance.MappedTo("test", String.Empty))
                .Should().Throw<ArgumentOutOfRangeException>().Which.ParamName.Should().Be("term");
        }

        [Test]
        public void Should_prepare_entity_data_model_correctly()
        {
            Builder.Classes.Should().Contain(new Tuple<Iri, Iri>(new Iri("term"), new Iri("graph")));
        }
    }
}
