using System;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Mapping.Providers;

namespace Given_instance_of.ConverterConventionVisitor_class
{
    [TestFixture]
    public class when_visiting_a_simple_property_mapping : ConverterConventionVisitorTest<IProduct, IPropertyMappingProvider>
    {
        protected override string PropertyName { get { return "Name"; } }

        public override void TheTest()
        {
            Visitor.Visit(Provider.Object);
        }

        [Test]
        public void Should_assign_a_converter_type()
        {
            Provider.VerifySet(instance => instance.ValueConverterType = Converter.Object.GetType(), Times.Once);
        }

        [Test]
        public void Should_obtain_converter_capabilities()
        {
            ConverterProvider.Verify(instance => instance.FindLiteralConverter(PropertyType), Times.Once);
        }

        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();
            Provider.SetupGet(instance => instance.ValueConverterType).Returns((Type)null);
        }
    }
}
