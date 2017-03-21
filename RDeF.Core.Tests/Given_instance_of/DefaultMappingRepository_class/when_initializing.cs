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
            MappingRepository = new DefaultMappingRepository(
                new[] { MappingSource.Object },
                new[] { MappingProviderVisitor.Object },
                new[] { new TestConverter() },
                Array.Empty<QIriMapping>());
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

        [Test]
        public void Should_call_the_mapping_provider_visitor_for_each_property()
        {
            foreach (var provider in ReverseCast<IPropertyMappingProvider>())
            {
                MappingProviderVisitor.Verify(instance => instance.Visit(provider), Times.Once);
            }
        }

        [Test]
        public void Should_call_the_mapping_provider_visitor_for_each_entity()
        {
            foreach (var provider in PrimaryEntityMappingProvider.Concat(SecondaryEntityMappingProvider).Select(item => item.Object).OfType<IEntityMappingProvider>())
            {
                MappingProviderVisitor.Verify(instance => instance.Visit(provider), Times.Once);
            }
        }

        protected override void ScenarioSetup()
        {
            PrimaryEntityMappingProvider = SetupMappingProviders("Product", "Name", "Price").ToList();
            SecondaryEntityMappingProvider = SetupMappingProviders("Service", "Name", "Description").ToList();
            MappingProviderVisitor.Setup(instance => instance.Visit(It.IsAny<IPropertyMappingProvider>()));
            MappingProviderVisitor.Setup(instance => instance.Visit(It.IsAny<IEntityMappingProvider>()));
            MappingSource.Setup(instance => instance.GatherEntityMappingProviders())
                .Returns(PrimaryEntityMappingProvider.Concat(SecondaryEntityMappingProvider).Select(provider => provider.Object));
        }

        private IEnumerable<T> ReverseCast<T>() where T : class, ITermMappingProvider
        {
            return PrimaryEntityMappingProvider.Concat(SecondaryEntityMappingProvider)
                .Select(item => item.Object)
                .OfType<T>();
        }
    }
}
