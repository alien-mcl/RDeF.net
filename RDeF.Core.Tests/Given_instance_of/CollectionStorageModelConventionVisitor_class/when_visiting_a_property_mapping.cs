using System;
using System.Reflection;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Mapping.Providers;

namespace Given_instance_of.CollectionStorageModelConventionVisitor_class
{
    [TestFixture]
    public class when_visiting_a_property_mapping : CollectionStorageModelConventionVisitorTest
    {
        public override void TheTest()
        {
            Visitor.Visit(Provider.Object);
        }

        [Test]
        public void Should_throw_when_no_collection_mapping_provider_is_given()
        {
            Visitor.Invoking(instance => instance.Visit((ICollectionMappingProvider)null))
                .Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Should_check_whether_the_provider_needs_a_storage_model_assigned()
        {
            Provider.VerifyGet(instance => instance.StoreAs, Times.Once);
        }

        [Test]
        public void Should_probe_mapped_property()
        {
            Provider.VerifyGet(instance => instance.Property, Times.Once);
        }

        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();
            Provider.SetupGet(instance => instance.Property).Returns(typeof(IProduct).GetTypeInfo().GetProperty("Categories"));
        }
    }
}
