using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Mapping;
using RDeF.Mapping.Converters;
using RDeF.Mapping.Explicit;

namespace Given_instance_of.DefaultExplicitMappingsBuilder_class
{
    [TestFixture]
    public class when_building_a_collection_mapping : DefaultExplicitMappingsBuilderTest
    {
        private IExplicitValueConverterBuilder<IProduct> ValueConverter { get; set; }

        public override void TheTest()
        {
            ValueConverter = Builder.WithCollection(product => product.Categories).MappedTo(new Iri("term"), new Iri("graph")).StoredAs(CollectionStorageModel.Simple);
        }

        [Test]
        public void Should_throw_when_no_collection_selector_is_given()
        {
            Builder.Invoking(instance => instance.WithCollection<IEnumerable<string>>(null)).ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Should_throw_when_the_selected_member_is_not_a_collection()
        {
            Builder.Invoking(instance => instance.WithCollection(product => product.Description)).ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Should_throw_when_no_mapped_term_is_given()
        {
            Builder.WithCollection(product => product.Categories).Invoking(instance => instance.MappedTo(null)).ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Should_prepare_collection_data_model_correctly_with_pointed_value_converter()
        {
            ((DefaultExplicitMappingsBuilder<IProduct>)ValueConverter.WithValueConverter<TestConverter>())
                .Collections.Should().ContainKey(typeof(IProduct).GetTypeInfo().GetProperty("Categories"))
                    .WhichValue.ShouldBeEquivalentTo(
                        new Tuple<Iri, Iri, CollectionStorageModel, Type>(new Iri("term"), new Iri("graph"), CollectionStorageModel.Simple, typeof(TestConverter)));
        }

        [Test]
        public void Should_prepare_collection_data_model_correctly_with_default_value_converter()
        {
            ((DefaultExplicitMappingsBuilder<IProduct>)ValueConverter.WithDefaultConverter())
                .Collections.Should().ContainKey(typeof(IProduct).GetTypeInfo().GetProperty("Categories"))
                .WhichValue.ShouldBeEquivalentTo(
                    new Tuple<Iri, Iri, CollectionStorageModel, Type>(new Iri("term"), new Iri("graph"), CollectionStorageModel.Simple, null));
        }
    }
}
