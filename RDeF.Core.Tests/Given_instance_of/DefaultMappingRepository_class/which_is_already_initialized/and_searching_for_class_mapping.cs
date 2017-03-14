using System;
using FluentAssertions;
using Moq;
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
            Result.Classes.Should().Contain(new Iri(ExpectedClass));
        }

        [Test]
        public void Should_throw_when_no_class_is_given()
        {
            MappingRepository.Invoking(instance => instance.FindEntityMappingFor((Iri)null)).ShouldThrow<ArgumentNullException>();
        }

        protected override void ScenarioSetup()
        {
            MappingSource.Setup(instance => instance.GatherEntityMappings())
                .Returns(new[] { SetupEntityMapping(new Mock<IConverter>(MockBehavior.Strict).Object, ExpectedClass, "Name", "Price").Object });
            base.ScenarioSetup();
        }
    }
}
