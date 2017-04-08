using System;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using RDeF;
using RDeF.Data;
using RDeF.Mapping;
using RDeF.Mapping.Converters;
using RDeF.Mapping.Visitors;

namespace Given_complex_mappings.DefaultMappingRepository_class
{
    [TestFixture]
    public class when_obtaining_a_generic_type_mappings
    {
        private DefaultMappingRepository Repository { get; set; }

        [TestCase(typeof(IProductOffering<IProduct>))]
        [TestCase(typeof(IProductOffering<ISpecialProduct>))]
        public void Should_provide_entity_mapping(Type genericEntityType)
        {
            Repository.FindEntityMappingFor(genericEntityType).Should().BeAssignableTo<IEntityMapping>().ContainMappingsFor(genericEntityType);
        }

        [SetUp]
        public void Setup()
        {
            var converters = new ILiteralConverter[]
            {
                new StringConverter(),
                new DecimalConverter(),
                new IntegerConverter(),
                new TestConverter(),
                new FloatingPointLiteralConverter(),
                new UntypedLiteralConverter()
            };
            var converterProvider = new DefaultConverterProvider(converters);
            var mappingProviderVisitors = new IMappingProviderVisitor[]
            {
                new CollectionStorageModelConventionVisitor()
            };
            Repository = new DefaultMappingRepository(
                new[] { new AttributesMappingSource(typeof(IProduct).GetTypeInfo().Assembly) },
                new DefaultMappingBuilder(converterProvider, Array.Empty<QIriMapping>(), mappingProviderVisitors));
        }
    }
}
