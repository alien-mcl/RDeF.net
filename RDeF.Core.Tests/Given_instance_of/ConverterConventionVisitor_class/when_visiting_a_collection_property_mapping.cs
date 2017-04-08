using System;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Mapping.Providers;

namespace Given_instance_of.ConverterConventionVisitor_class
{
    [TestFixture]
    public class when_visiting_a_collection_property_mapping : ConverterConventionVisitorTest<IProduct, ICollectionMappingProvider>
    {
        protected override string PropertyName { get { return "Categories"; } }

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
        public void Should_query_converter_provider_for_converter_instance()
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
