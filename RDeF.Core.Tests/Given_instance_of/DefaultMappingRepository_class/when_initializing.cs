using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Mapping;

namespace Given_instance_of.DefaultMappingRepository_class
{
    [TestFixture]
    public class when_initializing : DefaultMappingRepositoryTest
    {
        private Mock<IEntityMapping> PrimaryEntityMapping { get; set; }

        private Mock<IEntityMapping> SecondaryEntityMapping { get; set; }

        public override void TheTest()
        {
            MappingRepository = new DefaultMappingRepository(new[] { MappingSource.Object });
        }

        [Test]
        public void Should_merge_all_entity_mappings()
        {
            MappingRepository.Mappings.Should().HaveCount(1);
        }

        [Test]
        public void Should_merge_classes()
        {
            MappingRepository.Mappings[typeof(IProduct)].Classes.Should().HaveCount(2);
        }

        [Test]
        public void Should_merge_all_property_mappings()
        {
            MappingRepository.Mappings[typeof(IProduct)].Properties.Should().HaveCount(3);
        }

        protected override void ScenarioSetup()
        {
            var converter = new Mock<IConverter>(MockBehavior.Strict);
            PrimaryEntityMapping = SetupEntityMapping(converter.Object, "Product", "Name", "Price");
            SecondaryEntityMapping = SetupEntityMapping(converter.Object, "Service", "Name", "Description");
            MappingSource.Setup(instance => instance.GatherEntityMappings())
                .Returns(new[] { PrimaryEntityMapping.Object, SecondaryEntityMapping.Object });
        }
    }
}
