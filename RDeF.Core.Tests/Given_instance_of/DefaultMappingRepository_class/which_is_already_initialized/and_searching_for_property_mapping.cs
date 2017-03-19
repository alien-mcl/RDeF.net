using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Mapping;

namespace Given_instance_of.DefaultMappingRepository_class.which_is_already_initialized
{
    [TestFixture]
    public class and_searching_for_property_mapping : ScenarioTest
    {
        private static readonly PropertyInfo ExpectedProperty = typeof(IProduct).GetProperty("Name");

        private IPropertyMapping Result { get; set; }

        public override void TheTest()
        {
            Result = MappingRepository.FindPropertyMappingFor(ExpectedProperty);
        }

        [Test]
        public void Should_retrieve_the_property_mapping()
        {
            Result.Name.Should().Be(ExpectedProperty.Name);
        }

        [Test]
        public void Should_throw_when_no_property_is_given()
        {
            MappingRepository.Invoking(instance => instance.FindPropertyMappingFor((PropertyInfo)null)).ShouldThrow<ArgumentNullException>();
        }

        protected override void ScenarioSetup()
        {
            MappingSource.Setup(instance => instance.GatherEntityMappingProviders())
                .Returns(SetupMappingProviders("Product", ExpectedProperty.Name, "Price").Select(provider => provider.Object));
            base.ScenarioSetup();
        }
    }
}
