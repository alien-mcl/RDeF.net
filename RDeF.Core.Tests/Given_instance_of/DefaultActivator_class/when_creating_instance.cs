using System;
using FluentAssertions;
using NUnit.Framework;
using RDeF.ComponentModel;

namespace Given_instance_of.DefaultActivator_class
{
    [TestFixture]
    public class when_creating_instance
    {
        private IActivator Activator { get; set; }

        [Test]
        public void Should_create_instance_of_a_given_type()
        {
            Activator.CreateInstance(typeof(DefaultActivator)).Should().BeOfType<DefaultActivator>();
        }

        [Test]
        public void Should_create_instance_of_a_given_type_even_when_the_parameterless_constructor_is_not_public()
        {
            Activator.CreateInstance(typeof(InsufficientMemoryException)).Should().BeOfType<InsufficientMemoryException>();
        }

        [SetUp]
        public void Setup()
        {
            Activator = new DefaultActivator();
        }
    }
}
