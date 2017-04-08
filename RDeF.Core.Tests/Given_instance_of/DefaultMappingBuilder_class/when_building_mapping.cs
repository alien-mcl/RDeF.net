using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Mapping;
using RDeF.Mapping.Converters;
using RDeF.Mapping.Providers;

namespace Given_instance_of.DefaultMappingBuilder_class
{
    [TestFixture]
    public class when_building_mapping : DefaultMappingBuilderTest
    {
        private IEnumerable<Mock<ITermMappingProvider>> EntityMappingProvider { get; set; }

        private IDictionary<Type, IEntityMapping> Mappings { get; set; }

        public override void TheTest()
        {
            Builder.BuildMapping(Mappings, typeof(IProductOffering<IProduct>), OpenGenericMappingProviders[typeof(IProductOffering<>)]);
        }

        [Test]
        public void Should_create_new_entity_mapping()
        {
            Mappings.Should().HaveCount(1);
        }

        [Test]
        public void Should_provide_non_generic_type_property()
        {
            Mappings.First().Value.Properties.First().Name.Should().Be("Image");
        }

        [Test]
        public void Should_provide_generic_type_property()
        {
            Mappings.First().Value.Properties.Skip(1).First().Name.Should().Be("OfferedProduct");
        }

        [Test]
        public void Should_provide_non_generic_type_collection()
        {
            Mappings.First().Value.Properties.Last().Name.Should().Be("Texts");
        }

        protected override void ScenarioSetup()
        {
            ConverterProvider.Setup(instance => instance.FindConverter(It.IsAny<Type>())).Returns(new TestConverter());
            EntityMappingProvider = SetupMappingProviders(typeof(IProductOffering<>), "Offerring", "Image", "OfferedProduct", "Texts").ToList();
            MappingProviderVisitor.Setup(instance => instance.Visit(It.IsAny<ICollectionMappingProvider>()));
            MappingProviderVisitor.Setup(instance => instance.Visit(It.IsAny<IPropertyMappingProvider>()));
            MappingProviderVisitor.Setup(instance => instance.Visit(It.IsAny<IEntityMappingProvider>()));
            MappingSource.Setup(instance => instance.GatherEntityMappingProviders())
                .Returns(EntityMappingProvider.Select(provider => provider.Object));
            Mappings = Builder.BuildMappings(new[] { MappingSource.Object }, OpenGenericMappingProviders = new Dictionary<Type, ICollection<ITermMappingProvider>>());
        }
    }
}
