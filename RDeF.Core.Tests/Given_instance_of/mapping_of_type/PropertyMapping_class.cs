using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Entities;
using RDeF.Mapping;

namespace Given_instance_of.mapping_of_type
{
    [TestFixture]
    public class PropertyMapping_class
    {
        private const string ExpectedName = "Name";
        private static readonly Type ExpectedReturnType = typeof(string);
        private static readonly Iri ExpectedGraph = new Iri("iri");
        private static readonly Iri ExpectedPredicate = new Iri("predicate");

        private Mock<IConverter> Converter { get; set; }

        private Mock<IEntityMapping> EntityMapping { get; set; }

        private PropertyMapping Mapping { get; set; }

        [Test]
        public void Should_get_the_mapped_entity()
        {
            Mapping.EntityMapping.Should().Be(EntityMapping.Object);
        }

        [Test]
        public void Should_get_the_property_name()
        {
            Mapping.Name.Should().Be(ExpectedName);
        }

        [Test]
        public void Should_get_the_property_return_type()
        {
            Mapping.ReturnType.Should().Be(ExpectedReturnType);
        }

        [Test]
        public void Should_get_the_value_converter()
        {
            Mapping.ValueConverter.Should().Be(Converter.Object);
        }

        [Test]
        public void Should_get_the_graph()
        {
            Mapping.Graph.Should().Be(ExpectedGraph);
        }

        [Test]
        public void Should_get_the_predicate()
        {
            Mapping.Term.Should().Be(ExpectedPredicate);
        }

        [SetUp]
        public void Setup()
        {
            EntityMapping = new Mock<IEntityMapping>(MockBehavior.Strict);
            Converter = new Mock<IConverter>(MockBehavior.Strict);
            Mapping = new PropertyMapping(EntityMapping.Object, ExpectedName, ExpectedReturnType, ExpectedGraph, ExpectedPredicate, Converter.Object);
        }
    }
}
