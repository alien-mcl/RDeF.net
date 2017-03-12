using System.Reflection;
using Moq;
using NUnit.Framework;
using RDeF.ComponentModel;
using RDeF.Mapping;
using RDeF.Mapping.Attributes;

namespace Given_instance_of.AttributeMappingSource_class
{
    public abstract class AttributeMappingSourceTest
    {
        protected Mock<IActivator> Activator { get; private set; }

        protected AttributesMappingSource Source { get; private set; }

        public virtual void TheTest()
        {
        }

        [SetUp]
        public void Setup()
        {
            Activator = new Mock<IActivator>(MockBehavior.Strict);
            Source = new AttributesMappingSource(Activator.Object, GetType().GetTypeInfo().Assembly, new QIriMapping[0]);
            ScenarioSetup();
            TheTest();
        }

        protected virtual void ScenarioSetup()
        {
        }
    }
}
