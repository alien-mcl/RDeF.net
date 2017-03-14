using NUnit.Framework;
using RDeF.Entities;

namespace Given_instance_of.SimpleInMemoryEntitySource_class
{
    public abstract class SimpleInMemoryEntitySourceTest
    {
        protected SimpleInMemoryEntitySource EntitySource { get; private set; }

        public virtual void TheTest()
        {
        }

        [SetUp]
        public void Setup()
        {
            EntitySource = new SimpleInMemoryEntitySource();
            ScenarioSetup();
            TheTest();
        }

        protected virtual void ScenarioSetup()
        {
        }
    }
}
