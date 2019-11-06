using System;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Mapping;

namespace Given_instance_of.AttributeMappingSource_class
{
    [TestFixture]
    public class when_initializing : AttributeMappingSourceTest
    {
        [Test]
        public void Should_throw_when_no_assembly_is_given()
        {
            ((AttributesMappingSource)null).Invoking(_ => new AttributesMappingSource(null))
                .Should().Throw<ArgumentNullException>();
        }
    }
}
