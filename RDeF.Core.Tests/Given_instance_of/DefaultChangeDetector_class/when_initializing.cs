using System;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Entities;

namespace Given_instance_of.DefaultChangeDetector_class
{
    [TestFixture]
    public class when_initializing
    {
        [Test]
        public void Should_throw_when_no_mappings_repository_is_given()
        {
            ((DefaultChangeDetector)null).Invoking(_ => new DefaultChangeDetector(null))
                .Should().Throw<ArgumentNullException>();
        }
    }
}
