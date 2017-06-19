using System;
using System.Reflection;
#if NETSTANDARD1_6
using System.Reflection;
#endif
using FluentAssertions;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Mapping.Converters;
using RDeF.Mapping.Explicit;

namespace Given_instance_of.DefaultExplicitMappingsBuilder_class
{
    [TestFixture]
    public class when_building_a_property_mapping : DefaultExplicitMappingsBuilderTest
    {
        private IExplicitValueConverterBuilder<IProduct> ValueConverter { get; set; }

        public override void TheTest()
        {
            ValueConverter = Builder.WithProperty(product => product.Description).MappedTo(new Iri("term"), new Iri("graph"));
        }

        [Test]
        public void Should_throw_when_no_property_selector_is_given()
        {
            Builder.Invoking(instance => instance.WithProperty<string>(null)).ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Should_throw_when_invalid_property_selector_is_given()
        {
            Builder.Invoking(instance => instance.WithProperty(product => 1)).ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Should_throw_when_the_selected_member_is_a_collection()
        {
            Builder.Invoking(instance => instance.WithProperty(product => product.Categories)).ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Should_throw_when_no_mapped_term_is_given()
        {
            Builder.WithProperty(product => product.Description).Invoking(instance => instance.MappedTo(null)).ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Should_prepare_property_data_model_correctly_with_pointed_value_converter()
        {
            ((DefaultExplicitMappingsBuilder<IProduct>)ValueConverter.WithValueConverter<TestConverter>())
                .Properties.Should().ContainKey(typeof(IProduct).GetTypeInfo().GetProperty("Description"))
                .WhichValue.ShouldBeEquivalentTo(
                    new Tuple<Iri, Iri, Type>(new Iri("term"), new Iri("graph"), typeof(TestConverter)));
        }

        [Test]
        public void Should_prepare_property_data_model_correctly_with_default_value_converter()
        {
            ((DefaultExplicitMappingsBuilder<IProduct>)ValueConverter.WithDefaultConverter())
                .Properties.Should().ContainKey(typeof(IProduct).GetTypeInfo().GetProperty("Description"))
                .WhichValue.ShouldBeEquivalentTo(
                    new Tuple<Iri, Iri, Type>(new Iri("term"), new Iri("graph"), null));
        }
    }
}
