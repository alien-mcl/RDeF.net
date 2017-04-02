using System;
using Moq;
using NUnit.Framework;
using RDeF.ComponentModel;

namespace Given_instance_of.DefaultEntityContext_class
{
    public class when_resolving_a_component : DefaultEntityContextTest
    {
        private const string Expected = "test";

        private string Result { get; set; }

        public override void TheTest()
        {
            Result = ((IComponentScope)Context).Resolve<string>();
        }

        [Test]
        public void Should_call_the_factory_method()
        {
            Container.Verify(instance => instance.Resolve(typeof(string)), Times.Once);
        }

        protected override void ScenarioSetup()
        {
            Container.Setup(instance => instance.Resolve(It.IsAny<Type>())).Returns(Expected);
        }
    }
}
