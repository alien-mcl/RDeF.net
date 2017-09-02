using System.Reflection;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Mapping;

namespace Given_instance_of.mapping_of_type
{
    [TestFixture]
    public class CollectionMapping_class
    {
        private CollectionMapping Mapping { get; set; }

        [Test]
        public void Should_get_the_collection_storage_model()
        {
            Mapping.StoreAs.Should().Be(CollectionStorageModel.Simple);
        }

        [SetUp]
        public void Setup()
        {
            Mapping = new CollectionMapping(
                new Mock<IEntityMapping>(MockBehavior.Strict).Object,
                typeof(IProduct).GetTypeInfo().GetProperty("Name"),
                new Iri("iri"),
                new Iri("predicate"),
                new Mock<IConverter>(MockBehavior.Strict).Object,
                CollectionStorageModel.Simple);
        }
    }
}
