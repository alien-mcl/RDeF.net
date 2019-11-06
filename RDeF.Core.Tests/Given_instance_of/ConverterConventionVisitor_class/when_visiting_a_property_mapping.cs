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
            Visitor.Invoking(instance => instance.Visit((ICollectionMappingProvider)null))
                .Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Should_not_query_converter_provider_for_converter_instance()
        {
            ConverterProvider.Verify(instance => instance.FindLiteralConverter(It.IsAny<Type>()), Times.Never);
        }

        [Test]
        public void Should_not_assign_a_converter_type()
        {
            Provider.VerifySet(instance => instance.ValueConverterType = It.IsAny<Type>(), Times.Never);
        }

        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();
            Provider.SetupGet(instance => instance.ValueConverterType).Returns((Type)null);
        }
    }
}
