using System;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Serialization;

namespace Given_instance_of.Graph_class
{
    [TestFixture]
    public class when_initializing
    {
        [Test]
        public void Should_throw_when_no_statements_are_given()
        {
            ((Graph)null).Invoking(_ => new Graph(null)).Should().Throw<ArgumentNullException>()
                .Which.ParamName.Should().Be("statements");
        }

        [Test]
        public void Should_throw_when_no_iri_are_given()
        {
            ((Graph)null).Invoking(_ => new Graph(null, null)).Should().Throw<ArgumentNullException>()
                .Which.ParamName.Should().Be("iri");
        }
    }
}
