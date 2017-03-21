using System;
using System.Reflection;
using Moq;
using NUnit.Framework;
using RDeF.Mapping;
using RDeF.Mapping.Providers;
using RDeF.Mapping.Visitors;

namespace Given_instance_of.ConverterConventionVisitor_class
{
    public abstract class ConverterConventionVisitorTest<TEntity, TMappingProvider> where TMappingProvider : class, IPropertyMappingProvider
    {
        protected abstract string PropertyName { get; }

        protected Mock<ILiteralConverter> Converter { get; private set; }

        protected Mock<TMappingProvider> Provider { get; set; }

        protected ConverterConventionVisitor Visitor { get; private set; }

        public virtual void TheTest()
        {
        }

        [SetUp]
        public void Setup()
        {
            Converter = new Mock<ILiteralConverter>(MockBehavior.Strict);
            Visitor = new ConverterConventionVisitor(new[] { Converter.Object });
            ScenarioSetup();
            TheTest();
        }

        protected virtual void ScenarioSetup()
        {
            Provider = new Mock<TMappingProvider>(MockBehavior.Strict);
            Provider.SetupSet(instance => instance.ValueConverterType = It.IsAny<Type>());
            Provider.SetupGet(instance => instance.Property).Returns(typeof(TEntity).GetTypeInfo().GetProperty(PropertyName));
        }
    }
}
