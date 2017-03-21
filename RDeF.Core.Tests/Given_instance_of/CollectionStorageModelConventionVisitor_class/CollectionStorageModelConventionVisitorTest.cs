using Moq;
using NUnit.Framework;
using RDeF.Mapping;
using RDeF.Mapping.Providers;
using RDeF.Mapping.Visitors;

namespace Given_instance_of.CollectionStorageModelConventionVisitor_class
{
    public class CollectionStorageModelConventionVisitorTest
    {
        protected Mock<ICollectionMappingProvider> Provider { get; private set; }

        protected CollectionStorageModelConventionVisitor Visitor { get; private set; }

        public virtual void TheTest()
        {
        }

        [SetUp]
        public void Setup()
        {
            Visitor = new CollectionStorageModelConventionVisitor();
            ScenarioSetup();
            TheTest();
        }

        protected virtual void ScenarioSetup()
        {
            Provider = new Mock<ICollectionMappingProvider>(MockBehavior.Strict);
            Provider.SetupGet(instance => instance.StoreAs).Returns(CollectionStorageModel.Unspecified);
            Provider.SetupSet(instance => instance.StoreAs = It.IsAny<CollectionStorageModel>());
        }
    }
}
