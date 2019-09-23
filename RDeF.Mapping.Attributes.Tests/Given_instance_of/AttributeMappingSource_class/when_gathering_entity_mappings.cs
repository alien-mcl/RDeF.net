using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Mapping;
using RDeF.Mapping.Providers;

namespace Given_instance_of.AttributeMappingSource_class
{
    [TestFixture]
    public class when_gathering_entity_mappings : AttributeMappingSourceTest
    {
        private IEnumerable<ITermMappingProvider> Result { get; set; }

        public override void TheTest()
        {
            Result = Source.GatherEntityMappingProviders();
        }

        [Test]
        public void Should_gather_all_entity_mappings()
        {
            Result.Should().HaveCount(7);
        }

        [Test]
        public void Should_define_mapped_entity_type_correctly()
        {
            Result.OfType<IEntityMappingProvider>().First().EntityType.Should().Be(typeof(IProduct));
        }

        [Test]
        public void Should_gather_all_class_mappings_for_an_entity()
        {
            Result.OfType<IEntityMappingProvider>().Should().HaveCount(1)
                .And.Subject.First().GetTerm(Array.Empty<QIriMapping>()).ToString().Should().Be("some:Product");
        }

        [Test]
        public void Should_gather_all_property_mappings_for_an_entity()
        {
            Result.OfType<IPropertyMappingProvider>().Should().HaveCount(6);
        }

        [Test]
        public void Should_setup_all_properties_according_to_their_mappings()
        {
            Result.OfType<IPropertyMappingProvider>()
                .All(property => $"some:{property.Property.Name.ToLower()}" == property.GetTerm(Array.Empty<QIriMapping>()).ToString())
                .Should().BeTrue();
        }
    }
}
