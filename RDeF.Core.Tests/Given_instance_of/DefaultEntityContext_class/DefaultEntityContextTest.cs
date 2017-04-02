using System;
using Moq;
using NUnit.Framework;
using RDeF.Entities;
using RDeF.Mapping;

namespace Given_instance_of.DefaultEntityContext_class
{
    public abstract class DefaultEntityContextTest
    {
        public interface IScope
        {
            object Resolve(Type type);
        }

        internal Mock<IChangeDetector> ChangeDetector { get; private set; }

        protected Mock<IEntitySource> EntitySource { get; set; }

        protected Mock<IMappingsRepository> MappingsRepository { get; private set; }

        protected Mock<IScope> Container { get; set; }

        protected DefaultEntityContext Context { get; set; }

        public virtual void TheTest()
        {
        }

        [SetUp]
        public void Setup()
        {
            EntitySource = new Mock<IEntitySource>(MockBehavior.Strict);
            MappingsRepository = new Mock<IMappingsRepository>(MockBehavior.Strict);
            ChangeDetector = new Mock<IChangeDetector>(MockBehavior.Strict);
            Container = new Mock<IScope>(MockBehavior.Strict);
            Context = new DefaultEntityContext(
                EntitySource.Object,
                MappingsRepository.Object,
                ChangeDetector.Object,
                Container.Object.Resolve);
            ScenarioSetup();
            TheTest();
        }

        protected virtual void ScenarioSetup()
        {
        }
    }
}
