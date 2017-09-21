using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Mapping.Providers;

namespace Given_instance_of.FluentMappingSource_class
{
    [TestFixture]
    public class when_gathering_entity_mappings : FluentMappingSourceTest
    {
        private IEnumerable<ITermMappingProvider> Result { get; set; }

        public override void TheTest()
        {
            Result = Source.GatherEntityMappingProviders();
        }

        [Test]
        public void Should_gather_all_entity_mappings()
        {
            Result.Should().HaveCount(9);
        }

        [Test]
        public void Should_define_mapped_entity_type_correctly()
        {
            Result.OfType<IEntityMappingProvider>().First().EntityType.Should().Be(typeof(IUnmappedProduct));
        }

        [Test]
        public void Should_gather_all_class_mappings_for_an_entity()
        {
            Result.OfType<IEntityMappingProvider>().Should().HaveCount(3).And.Subject.Last().GetTerm(new[] { QIriMapping }).ToString().Should().Be("test_term");
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
                .All(property => property.Property.Name.ToLower() == property.GetTerm(new[] { QIriMapping }).ToString().Replace("test_", String.Empty))
                .Should().BeTrue();
        }
    }
}
