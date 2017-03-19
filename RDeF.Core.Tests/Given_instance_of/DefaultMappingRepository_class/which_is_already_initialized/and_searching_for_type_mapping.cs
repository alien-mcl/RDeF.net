using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Mapping;

namespace Given_instance_of.DefaultMappingRepository_class.which_is_already_initialized
{
    [TestFixture]
    public class and_searching_for_type_mapping : ScenarioTest
    {
        private static readonly Type ExpectedType = typeof(IProduct);

        private IEntityMapping Result { get; set; }

        public override void TheTest()
        {
            Result = MappingRepository.FindEntityMappingFor(ExpectedType);
        }

        [Test]
        public void Should_retrieve_the_entity_mapping()
        {
            Result.Type.Should().Be(ExpectedType);
        }

        [Test]
        public void Should_throw_when_no_type_is_given()
        {
            MappingRepository.Invoking(instance => instance.FindEntityMappingFor((Type)null)).ShouldThrow<ArgumentNullException>();
        }

        protected override void ScenarioSetup()
        {
            MappingSource.Setup(instance => instance.GatherEntityMappingProviders())
                .Returns(SetupMappingProviders("Product", "Name", "Price").Select(provider => provider.Object));
            base.ScenarioSetup();
        }
    }
}
