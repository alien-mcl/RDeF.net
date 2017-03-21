using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Mapping.Providers;

namespace Given_instance_of.ConverterConventionVisitor_class
{
    [TestFixture]
    public class when_visiting_a_property_mapping : ConverterConventionVisitorTest<IComplexEntity, ICollectionMappingProvider>
    {
        protected override string PropertyName { get { return "Related"; } }

        public override void TheTest()
        {
            Visitor.Visit(Provider.Object);
        }

        [Test]
        public void Should_throw_when_no_provider_is_given()
        {
            Visitor.Invoking(instance => instance.Visit((ICollectionMappingProvider)null)).ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Should_check_whether_the_provider_needs_a_converter_type_assigned()
        {
            Provider.VerifyGet(instance => instance.ValueConverterType, Times.Once);
        }

        [Test]
        public void Should_probe_mapped_property()
        {
            Provider.VerifyGet(instance => instance.Property, Times.Once);
        }

        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();
            Provider.SetupGet(instance => instance.ValueConverterType).Returns((Type)null);
        }
    }
}
