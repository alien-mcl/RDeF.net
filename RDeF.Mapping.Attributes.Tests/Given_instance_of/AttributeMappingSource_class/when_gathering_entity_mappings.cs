using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Mapping;

namespace Given_instance_of.AttributeMappingSource_class
{
    [TestFixture]
    public class when_gathering_entity_mappings : AttributeMappingSourceTest
    {
        private IEnumerable<IEntityMapping> Result { get; set; }

        public override void TheTest()
        {
            Result = Source.GatherEntityMappings();
        }

        [Test]
        public void Should_gather_all_entity_mappings()
        {
            Result.Should().HaveCount(1);
        }

        [Test]
        public void Should_define_mapped_entity_type_correctly()
        {
            Result.First().Type.Should().Be(typeof(IProduct));
        }

        [Test]
        public void Should_gather_all_class_mappings_for_an_entity()
        {
            Result.First().Classes.Should().HaveCount(1);
        }

        [Test]
        public void Should_gather_all_property_mappings_for_an_entity()
        {
            Result.First().Properties.Should().HaveCount(4);
        }

        [Test]
        public void Should_setup_all_properties_according_to_their_mappings()
        {
            Result.First().Properties.All(property => property.Name.ToLower() == property.Predicate.ToString()).Should().BeTrue();
        }

        [Test]
        public void Should_initialize_mapped_converter_correctly()
        {
            Activator.Verify(instance => instance.CreateInstance(typeof(TestConverter)), Times.Once);
        }

        protected override void ScenarioSetup()
        {
            Activator.Setup(instance => instance.CreateInstance(It.IsAny<Type>())).Returns(new TestConverter());
        }
    }
}
