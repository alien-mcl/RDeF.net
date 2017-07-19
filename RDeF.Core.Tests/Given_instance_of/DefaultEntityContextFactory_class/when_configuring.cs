using System;
using System.Reflection;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.ComponentModel;
using RDeF.Entities;
using RDeF.Mapping;

namespace Given_instance_of.DefaultEntityContextFactory_class
{
    public class when_configuring : DefaultEntityContextFactoryTest
    {
        private Mock<IEntitySource> EntitySource { get; set; }

        private bool TestComponentIsActivated { get; set; }

        public override void TheTest()
        {
            ((IContainer)((IComponentConfigurator)Factory.WithEntitySource(EntitySource.Object))
                .WithComponent<IDisposable, TestComponent>(null, Lifestyle.Singleton, (scope, instance) => TestComponentIsActivated = true)
                .GetType().GetField("_container", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Factory)).Resolve<IDisposable>();
        }

        [Test]
        public void Should_get_mappings()
        {
            Factory.Mappings.Should().BeAssignableTo<IMappingsRepository>();
        }

        [Test]
        public void Should_dispose_registered_component()
        {
            EntitySourceFromDisposedFactory().As<IDisposable>().Verify(instance => instance.Dispose(), Times.Once);
        }

        [Test]
        public void Should_call_a_handler_when_component_is_activated()
        {
            TestComponentIsActivated.Should().BeTrue();
        }

        protected override void ScenarioSetup()
        {
            EntitySource = new Mock<IEntitySource>(MockBehavior.Strict);
            EntitySource.As<IDisposable>().Setup(instance => instance.Dispose());
        }

        private Mock<IEntitySource> EntitySourceFromDisposedFactory()
        {
            Factory.Dispose();
            return EntitySource;
        }

        public sealed class TestComponent : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}
