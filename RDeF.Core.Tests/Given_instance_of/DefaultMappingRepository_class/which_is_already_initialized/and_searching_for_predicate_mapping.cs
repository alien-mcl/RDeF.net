using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Entities;
using RDeF.Mapping;

namespace Given_instance_of.DefaultMappingRepository_class.which_is_already_initialized
{
    [TestFixture]
    public class and_searching_for_predicate_mapping : ScenarioTest
    {
        private const string ExpectedProperty = "Name";

        private IPropertyMapping Result { get; set; }

        public override void TheTest()
        {
            Result = MappingRepository.FindPropertyMappingFor(new Iri(ExpectedProperty));
        }

        [Test]
        public void Should_retrieve_the_property_mapping()
        {
            Result.Name.Should().Be(ExpectedProperty);
        }

        [Test]
        public void Should_throw_when_no_predicate_is_given()
        {
            MappingRepository.Invoking(instance => instance.FindPropertyMappingFor((Iri)null)).ShouldThrow<ArgumentNullException>();
        }

        protected override void ScenarioSetup()
        {
            MappingSource.Setup(instance => instance.GatherEntityMappings())
                .Returns(new[] { SetupEntityMapping(new Mock<IConverter>(MockBehavior.Strict).Object, "Product", ExpectedProperty, "Price").Object });
            base.ScenarioSetup();
        }
    }
}
