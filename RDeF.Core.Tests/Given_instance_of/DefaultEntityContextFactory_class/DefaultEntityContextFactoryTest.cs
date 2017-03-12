using NUnit.Framework;
using RDeF.Entities;

namespace Given_instance_of.DefaultEntityContextFactory_class
{
    public abstract class DefaultEntityContextFactoryTest
    {
        protected DefaultEntityContextFactory Factory { get; private set; }

        public virtual void TheTest()
        {
        }

        [SetUp]
        public void Setup()
        {
            Factory = new DefaultEntityContextFactory();
            ScenarioSetup();
            TheTest();
        }

        protected virtual void ScenarioSetup()
        {
        }
    }
}
