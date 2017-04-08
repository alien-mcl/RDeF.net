using FluentAssertions;
using NUnit.Framework;
using RDeF.Mapping.Converters;

namespace Given_instance_of.DefaultConverterProvider_class
{
    [TestFixture]
    public class when_searching_for_converters : DefaultConverterProviderTest
    {
        [Test]
        public void Should_provide_best_suited_converter()
        {
            Provider.FindConverter(typeof(SpecializedTestConverter)).Should().Be(Converter);
        }

        [Test]
        public void Should_provide_matching_converter()
        {
            Provider.FindConverter(typeof(TestConverter)).Should().Be(Converter);
        }

        [Test]
        public void Should_provide_converter_supporting_required_value_type()
        {
            Provider.FindLiteralConverter(typeof(object)).Should().Be(Converter);
        }

        [Test]
        public void Should_provide_a_fallback_converter()
        {
            Provider.FindLiteralConverter(typeof(string)).Should().Be(FallbackConverter.Object);
        }
    }
}
