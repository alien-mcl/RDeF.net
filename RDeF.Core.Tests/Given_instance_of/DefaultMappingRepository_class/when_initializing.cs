using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Mapping;
using RDeF.Mapping.Providers;

namespace Given_instance_of.DefaultMappingRepository_class
{
    [TestFixture]
    public class when_initializing : DefaultMappingRepositoryTest
    {
        private IEnumerable<Mock<ITermMappingProvider>> PrimaryEntityMappingProvider { get; set; }

        private IEnumerable<Mock<ITermMappingProvider>> SecondaryEntityMappingProvider { get; set; }

        public override void TheTest()
        {
            MappingRepository = new DefaultMappingRepository(new[] { MappingSource.Object }, new[] { new TestConverter() }, Array.Empty<QIriMapping>());
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
            PrimaryEntityMappingProvider = SetupMappingProviders("Product", "Name", "Price");
            SecondaryEntityMappingProvider = SetupMappingProviders("Service", "Name", "Description");
            MappingSource.Setup(instance => instance.GatherEntityMappingProviders())
                .Returns(PrimaryEntityMappingProvider.Concat(SecondaryEntityMappingProvider).Select(provider => provider.Object));
        }
    }
}
