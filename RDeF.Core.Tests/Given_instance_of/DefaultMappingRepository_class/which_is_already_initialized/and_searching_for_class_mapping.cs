using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Entities;
using RDeF.Mapping;

namespace Given_instance_of.DefaultMappingRepository_class.which_is_already_initialized
{
    [TestFixture]
    public class and_searching_for_class_mapping : ScenarioTest
    {
        private const string ExpectedClass = "Product";

        private IEntityMapping Result { get; set; }

        public override void TheTest()
        {
            Result = MappingRepository.FindEntityMappingFor(new Iri(ExpectedClass));
        }

        [Test]
        public void Should_retrieve_the_entity_mapping()
        {
            Result.Classes.Where(@class => @class.Term == new Iri(ExpectedClass)).Should().HaveCount(1);
        }

        [Test]
        public void Should_throw_when_no_class_is_given()
        {
            MappingRepository.Invoking(instance => instance.FindEntityMappingFor((Iri)null)).ShouldThrow<ArgumentNullException>();
        }

        protected override void ScenarioSetup()
        {
            MappingSource.Setup(instance => instance.GatherEntityMappingProviders())
                .Returns(SetupMappingProviders(ExpectedClass, "Name", "Price").Select(provider => provider.Object));
            base.ScenarioSetup();
        }
    }
}
