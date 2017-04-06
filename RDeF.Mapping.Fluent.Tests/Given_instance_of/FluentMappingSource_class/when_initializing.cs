using System;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Mapping;

namespace Given_instance_of.FluentMappingSource_class
{
    [TestFixture]
    public class when_initializing : FluentMappingSourceTest
    {
        [Test]
        public void Should_throw_when_no_assembly_is_given()
        {
            ((FluentMappingSource)null).Invoking(_ => new FluentMappingSource(null)).ShouldThrow<ArgumentNullException>();
        }
    }
}
