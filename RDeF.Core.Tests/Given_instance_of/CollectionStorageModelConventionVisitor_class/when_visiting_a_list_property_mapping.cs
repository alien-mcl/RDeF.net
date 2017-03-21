using System.Reflection;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Mapping;

namespace Given_instance_of.CollectionStorageModelConventionVisitor_class
{
    [TestFixture]
    public class when_visiting_a_list_property_mapping : CollectionStorageModelConventionVisitorTest
    {
        public override void TheTest()
        {
            Visitor.Visit(Provider.Object);
        }

        [Test]
        public void Should_assign_a_storage_model()
        {
            Provider.VerifySet(instance => instance.StoreAs = CollectionStorageModel.LinkedList, Times.Once);
        }

        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();
            Provider.SetupGet(instance => instance.Property).Returns(typeof(IProduct).GetTypeInfo().GetProperty("Comments"));
        }
    }
}
