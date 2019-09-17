using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RDeF.Entities;
using RDeF.Mapping;

namespace Given_instance_of.SimpleInMemoryEntitySource_class
{
    public abstract class SimpleInMemoryEntitySourceTest
    {
        protected Mock<DefaultEntityContext> Context { get; private set; }

        protected SimpleInMemoryEntitySource EntitySource { get; private set; }

        public virtual Task TheTest()
        {
            return Task.CompletedTask;
        }

        [SetUp]
        public async Task Setup()
        {
            EntitySource = new SimpleInMemoryEntitySource(() => Context.Object);
            ScenarioSetup();
            await TheTest();
        }

        protected virtual void ScenarioSetup()
        {
            Context = new Mock<DefaultEntityContext>(
                MockBehavior.Strict,
                EntitySource,
                new Mock<IMappingsRepository>(MockBehavior.Strict).Object,
                new Mock<IChangeDetector>(MockBehavior.Strict).Object,
                Array.Empty<ILiteralConverter>());
        }
    }
}
