using System;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Mapping;

namespace Given_instance_of.InternalMappingSource_class
{
    [TestFixture]
    public class when_initializing : InternalMappingSourceTest
    {
        [Test]
        public void Should_throw_when_no_assembly_is_given()
        {
            ((InternalMappingSource)null).Invoking(_ => new InternalMappingSource(null))
                .Should().Throw<ArgumentNullException>();
        }
    }
}
