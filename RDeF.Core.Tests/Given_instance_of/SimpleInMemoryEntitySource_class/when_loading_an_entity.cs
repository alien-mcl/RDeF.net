using System;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Entities;

namespace Given_instance_of.SimpleInMemoryEntitySource_class
{
    [TestFixture]
    public class when_loading_an_entity : SimpleInMemoryEntitySourceTest
    {
        [Test]
        public void Should_throw()
        {
            EntitySource.Invoking(instance => instance.Load(new Iri("test"))).ShouldThrow<NotSupportedException>();
        }
    }
}
