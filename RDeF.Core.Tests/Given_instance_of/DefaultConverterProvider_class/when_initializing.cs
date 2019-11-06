using System;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Mapping;

namespace Given_instance_of.DefaultConverterProvider_class
{
    [TestFixture]
    public class when_initializing
    {
        [Test]
        public void Should_throw_when_no_converters_are_given()
        {
            ((DefaultConverterProvider)null).Invoking(_ => new DefaultConverterProvider(null))
                .Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Should_throw_given_collection_of_converters_is_empty()
        {
            ((DefaultConverterProvider)null).Invoking(_ => new DefaultConverterProvider(Array.Empty<ILiteralConverter>()))
                .Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
