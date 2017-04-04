using Moq;
using NUnit.Framework;
using RDeF.Entities;
using RDeF.Mapping;

namespace Given_instance_of.SimpleInMemoryEntitySource_class
{
    public abstract class SimpleInMemoryEntitySourceTest
    {
        protected DefaultEntityContext Context { get; private set; }

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
            Context = new DefaultEntityContext(
                EntitySource,
                new Mock<IMappingsRepository>(MockBehavior.Strict).Object,
                new Mock<IChangeDetector>(MockBehavior.Strict).Object);
        }
    }
}
