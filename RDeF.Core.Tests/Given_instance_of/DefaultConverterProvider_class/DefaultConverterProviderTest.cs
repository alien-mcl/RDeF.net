using System;
using Moq;
using NUnit.Framework;
using RDeF.Entities;
using RDeF.Mapping;
using RDeF.Mapping.Converters;

namespace Given_instance_of.DefaultConverterProvider_class
{
    public abstract class DefaultConverterProviderTest
    {
        protected SpecializedTestConverter Converter { get; private set; }

        protected Mock<ILiteralConverter> AuxConverter { get; private set; }

        protected Mock<ILiteralConverter> FallbackConverter { get; private set; }

        protected DefaultConverterProvider Provider { get; private set; }

        [SetUp]
        public void Setup()
        {
            Converter = new SpecializedTestConverter();
            AuxConverter = new Mock<ILiteralConverter>(MockBehavior.Strict);
            AuxConverter.SetupGet(instance => instance.SupportedTypes).Returns(new[] { typeof(void) });
            AuxConverter.SetupGet(instance => instance.SupportedDataTypes).Returns(Array.Empty<Iri>());
            FallbackConverter = new Mock<ILiteralConverter>(MockBehavior.Strict);
            FallbackConverter.SetupGet(instance => instance.SupportedTypes).Returns(Type.EmptyTypes);
            FallbackConverter.SetupGet(instance => instance.SupportedDataTypes).Returns(Array.Empty<Iri>());
            Provider = new DefaultConverterProvider(new[] { Converter, AuxConverter.Object, FallbackConverter.Object });
        }

        public class SpecializedTestConverter : TestConverter
        {
        }
    }
}
