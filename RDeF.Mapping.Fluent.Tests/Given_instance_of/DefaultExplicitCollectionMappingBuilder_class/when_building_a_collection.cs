using System;
using System.Reflection;
#if NETSTANDARD1_6
using System.Reflection;
#endif
using FluentAssertions;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Mapping.Explicit;

namespace Given_instance_of.DefaultExplicitCollectionMappingBuilder_class
{
    [TestFixture]
    public class when_building_a_collection
    {
        private DefaultExplicitCollectionMappingBuilder<IUnmappedProduct> Builder { get; set; }

        [Test]
        public void Should_throw_when_no_prefix_is_given()
        {
            Builder.Invoking(instance => instance.MappedTo((string)null, null)).ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("prefix");
        }

        [Test]
        public void Should_throw_when_a_given_prefix_is_empty()
        {
            Builder.Invoking(instance => instance.MappedTo(String.Empty, null)).ShouldThrow<ArgumentOutOfRangeException>().Which.ParamName.Should().Be("prefix");
        }

        [Test]
        public void Should_throw_when_no_term_is_given()
        {
            Builder.Invoking(instance => instance.MappedTo("test", null)).ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("term");
        }

        [Test]
        public void Should_throw_when_a_given_term_is_empty()
        {
            Builder.Invoking(instance => instance.MappedTo("test", String.Empty)).ShouldThrow<ArgumentOutOfRangeException>().Which.ParamName.Should().Be("term");
        }

        [SetUp]
        public void Setup()
        {
            Builder = new DefaultExplicitCollectionMappingBuilder<IUnmappedProduct>(
                new DefaultExplicitMappingsBuilder<IUnmappedProduct>(),
                typeof(IUnmappedProduct).GetTypeInfo().GetProperty("Comments"));
        }
    }
}
