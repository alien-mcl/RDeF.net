using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.ComponentModel;
using RDeF.Mapping.Attributes;

namespace Given_instance_of.AttributeMappingSource_class
{
    [TestFixture]
    public class when_initializing : AttributeMappingSourceTest
    {
        [Test]
        public void Should_throw_when_no_activator_is_given()
        {
            ((AttributesMappingSource)null).Invoking(_ => new AttributesMappingSource(null, null, null))
                .ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("activator");
        }

        [Test]
        public void Should_throw_when_no_assembly_is_given()
        {
            ((AttributesMappingSource)null).Invoking(_ => new AttributesMappingSource(new Mock<IActivator>().Object, null, null))
                .ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("assembly");
        }
    }
}
