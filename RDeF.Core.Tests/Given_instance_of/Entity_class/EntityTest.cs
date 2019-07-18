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

        protected Mock<IMappingsRepository> MappingsRepository { get; private set; }

        protected Mock<IEntitySource> EntitySource { get; private set; }

        protected Iri Iri { get; private set; }

        protected Mock<DefaultEntityContext> Context { get; private set; }

        protected Mock<ILiteralConverter> LiteralConverter { get; private set; }

        public virtual void TheTest()
        {
        }

        [SetUp]
        public void Setup()
        {
            MappingsRepository = new Mock<IMappingsRepository>(MockBehavior.Strict);
            EntitySource = new Mock<IEntitySource>(MockBehavior.Strict);
            LiteralConverter = new Mock<ILiteralConverter>();
            Context = new Mock<DefaultEntityContext>(
                MockBehavior.Loose,
                EntitySource.Object,
                MappingsRepository.Object,
                new Mock<IChangeDetector>(MockBehavior.Strict).Object,
                new[] { LiteralConverter.Object });
            Context.SetupGet(instance => instance.Mappings).Returns(MappingsRepository.Object);
            Entity = new Entity(Iri = new Iri(new Uri("http://test.com/")), Context.Object);
            ScenarioSetup();
            TheTest();
        }

        protected virtual void ScenarioSetup()
        {
        }
    }
}
