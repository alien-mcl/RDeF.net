using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using RDeF;
using RDeF.Data;
using RDeF.Mapping;
using RDeF.Mapping.Converters;
using RDeF.Mapping.Providers;
using RDeF.Mapping.Visitors;

namespace Given_complex_mappings.DefaultMappingBuilder_class
{
    [TestFixture]
    public class when_building_mappings
    {
        private DefaultMappingBuilder Builder { get; set; }

        private IDictionary<Type, IEntityMapping> Mappings { get; set; }

        private IDictionary<Type, ICollection<ITermMappingProvider>> OpenGenericMappingProviders { get; set; }

        public void TheTest()
        {
            Mappings = Builder.BuildMappings(new AttributesMappingSourceProvider().GetMappingSourcesFor(typeof(IProduct).GetTypeInfo().Assembly), OpenGenericMappingProviders);
        }

        [TestCase(typeof(IThing))]
        [TestCase(typeof(IService))]
        [TestCase(typeof(IProduct))]
        [TestCase(typeof(ISpecialProduct))]
        public void Should_contain_mappings_for(Type type)
        {
            Mappings.Values.Should().ContainMappingsFor(type);
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
            var mappingProviderVisitors = new IMappingProviderVisitor[]
            {
                new CollectionStorageModelConventionVisitor(),
                new ConverterConventionVisitor(converters)
            };
            Builder = new DefaultMappingBuilder(converters, new QIriMapping[0], mappingProviderVisitors);
            OpenGenericMappingProviders = new Dictionary<Type, ICollection<ITermMappingProvider>>();
            TheTest();
        }
    }
}
