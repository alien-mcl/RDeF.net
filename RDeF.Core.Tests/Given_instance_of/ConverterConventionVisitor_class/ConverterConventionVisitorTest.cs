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

        protected PropertyInfo Property
        {
            get { return typeof(TEntity).GetTypeInfo().GetProperty(PropertyName); }
        }

        protected Type PropertyType
        {
            get
            {
                var result = Property.PropertyType;
                if (result.IsGenericType)
                {
                    return result.GetGenericArguments()[0];
                }

                return result;
            }
        }

        protected Mock<ILiteralConverter> Converter { get; private set; }

        protected Mock<IConverterProvider> ConverterProvider { get; private set; }

        protected Mock<TMappingProvider> Provider { get; set; }

        protected ConverterConventionVisitor Visitor { get; private set; }

        public virtual void TheTest()
        {
        }

        [SetUp]
        public void Setup()
        {
            Converter = new Mock<ILiteralConverter>(MockBehavior.Strict);
            ConverterProvider = new Mock<IConverterProvider>(MockBehavior.Strict);
            ConverterProvider.Setup(instance => instance.FindLiteralConverter(PropertyType)).Returns(Converter.Object);
            Visitor = new ConverterConventionVisitor(ConverterProvider.Object);
            ScenarioSetup();
            TheTest();
        }

        protected virtual void ScenarioSetup()
        {
            Provider = new Mock<TMappingProvider>(MockBehavior.Strict);
            Provider.SetupSet(instance => instance.ValueConverterType = It.IsAny<Type>());
            Provider.SetupGet(instance => instance.Property).Returns(Property);
        }
    }
}
