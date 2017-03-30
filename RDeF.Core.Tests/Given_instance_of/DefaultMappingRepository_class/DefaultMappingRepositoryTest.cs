using Moq;
using NUnit.Framework;
using RDeF.Mapping;

namespace Given_instance_of.DefaultMappingRepository_class
{
    public abstract class DefaultMappingRepositoryTest
    {
        protected DefaultMappingRepository MappingRepository { get; set; }

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
            MappingRepository = new DefaultMappingRepository(new[] { MappingSource.Object }, MappingBuilder.Object);
            TheTest();
        }

        protected virtual void ScenarioSetup()
        {
        }
    }
}
