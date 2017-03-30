using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Mapping;
using RDeF.Mapping.Providers;

namespace Given_instance_of.DefaultMappingBuilder_class
{
    [TestFixture]
    public class when_building_mappings : DefaultMappingBuilderTest
    {
        private IEnumerable<Mock<ITermMappingProvider>> PrimaryEntityMappingProvider { get; set; }

        private IEnumerable<Mock<ITermMappingProvider>> SecondaryEntityMappingProvider { get; set; }

        private IDictionary<Type, IEntityMapping> Result { get; set; }

        public override void TheTest()
        {
            Result = Builder.BuildMappings(new[] { MappingSource.Object }, OpenGenericMappingProviders);
        }

        [Test]
        public void Should_merge_all_entity_mappings()
        {
            Result.Should().HaveCount(1);
        }

        [Test]
        public void Should_merge_classes()
        {
            Result[typeof(IProduct)].Classes.Should().HaveCount(2);
        }

        [Test]
        public void Should_merge_all_property_mappings()
        {
            Result[typeof(IProduct)].Properties.Should().HaveCount(3);
        }

        [Test]
        public void Should_call_the_mapping_provider_visitor_for_each_property()
        {
            foreach (var provider in MappingsOfType<IPropertyMappingProvider>())
            {
                provider.Verify(instance => instance.Accept(MappingProviderVisitor.Object));
            }
        }

        [Test]
        public void Should_call_the_mapping_provider_visitor_for_each_entity()
        {
            foreach (var provider in MappingsOfType<IEntityMappingProvider>())
            {
                provider.Verify(instance => instance.Accept(MappingProviderVisitor.Object));
            }
        }

        [Test]
        public void Should_call_the_mapping_provider_visitor_for_each_collection()
        {
            foreach (var provider in MappingsOfType<ICollectionMappingProvider>())
            {
                provider.Verify(instance => instance.Accept(MappingProviderVisitor.Object));
            }
        }

        protected override void ScenarioSetup()
        {
            PrimaryEntityMappingProvider = SetupMappingProviders<IProduct>("Product", "Name", "Price").ToList();
            SecondaryEntityMappingProvider = SetupMappingProviders<IProduct>("Service", "Description", "Name").ToList();
            MappingProviderVisitor.Setup(instance => instance.Visit(It.IsAny<ICollectionMappingProvider>()));
            MappingProviderVisitor.Setup(instance => instance.Visit(It.IsAny<IPropertyMappingProvider>()));
            MappingProviderVisitor.Setup(instance => instance.Visit(It.IsAny<IEntityMappingProvider>()));
            MappingSource.Setup(instance => instance.GatherEntityMappingProviders())
                .Returns(PrimaryEntityMappingProvider.Concat(SecondaryEntityMappingProvider).Select(provider => provider.Object));
        }

        private IEnumerable<Mock<ITermMappingProvider>> MappingsOfType<T>() where T : class, ITermMappingProvider
        {
            return PrimaryEntityMappingProvider.Concat(SecondaryEntityMappingProvider)
                .Where(item => item.Object is T);
        }
    }
}
