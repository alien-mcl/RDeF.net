using System;
using Moq;
using NUnit.Framework;
using RDeF.Entities;
using RDeF.Mapping;

namespace Given_instance_of.Entity_class
{
    public abstract class EntityTest
    {
        internal Entity Entity { get; private set; }

        protected Iri Iri { get; private set; }

        protected Mock<DefaultEntityContext> Context { get; private set; }

        public virtual void TheTest()
        {
        }

        [SetUp]
        public void Setup()
        {
            Context = new Mock<DefaultEntityContext>(
                MockBehavior.Strict,
                new Mock<IEntitySource>(MockBehavior.Strict).Object,
                new Mock<IMappingsRepository>(MockBehavior.Strict).Object,
                new Mock<IChangeDetector>(MockBehavior.Strict).Object);
            Entity = new Entity(Iri = new Iri(new Uri("http://test.com/")), Context.Object);
            ScenarioSetup();
            TheTest();
        }

        protected virtual void ScenarioSetup()
        {
        }
    }
}
