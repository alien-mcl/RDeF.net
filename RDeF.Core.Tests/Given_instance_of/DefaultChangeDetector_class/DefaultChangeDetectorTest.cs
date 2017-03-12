using Moq;
using NUnit.Framework;
using RDeF.Entities;
using RDeF.Mapping;

namespace Given_instance_of.DefaultChangeDetector_class
{
    internal class DefaultChangeDetectorTest
    {
        protected Mock<IMappingsRepository> MappingsRepository { get; private set; }

        protected DefaultChangeDetector Detector { get; private set; }

        public virtual void TheTest()
        {
        }

        [SetUp]
        public void Setup()
        {
            MappingsRepository = new Mock<IMappingsRepository>(MockBehavior.Strict);
            Detector = new DefaultChangeDetector(MappingsRepository.Object);
            ScenarioSetup();
            TheTest();
        }

        protected virtual void ScenarioSetup()
        {
        }
    }
}
