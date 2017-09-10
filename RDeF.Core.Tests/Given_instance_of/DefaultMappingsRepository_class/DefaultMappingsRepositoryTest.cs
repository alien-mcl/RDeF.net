using Moq;
using NUnit.Framework;
using RDeF.Mapping;

namespace Given_instance_of.DefaultMappingsRepository_class
{
    public abstract class DefaultMappingsRepositoryTest
    {
        protected DefaultMappingsRepository MappingsRepository { get; set; }

        protected Mock<IMappingBuilder> MappingBuilder { get; private set; }

        protected Mock<IMappingSource> MappingSource { get; private set; }

        public virtual void TheTest()
        {
        }

        [SetUp]
        public void Setup()
        {
            MappingBuilder = new Mock<IMappingBuilder>(MockBehavior.Strict);
            MappingSource = new Mock<IMappingSource>(MockBehavior.Strict);
            ScenarioSetup();
            MappingsRepository = new DefaultMappingsRepository(new[] { MappingSource.Object }, MappingBuilder.Object);
            TheTest();
        }

        protected virtual void ScenarioSetup()
        {
        }
    }
}
