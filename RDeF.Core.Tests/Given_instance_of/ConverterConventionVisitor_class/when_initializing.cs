using System;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Mapping.Visitors;

namespace Given_instance_of.ConverterConventionVisitor_class
{
    [TestFixture]
    public class when_initializing
    {
        [Test]
        public void Should_throw_when_no_converters_are_given()
        {
            ((ConverterConventionVisitor)null)
                .Invoking(_ => new ConverterConventionVisitor(null))
                .Should().Throw<ArgumentNullException>();
        }
    }
}
