using FluentAssertions;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Mapping;

namespace Given_instance_of.mapping_of_type
{
    [TestFixture]
    public class MergingEntityMapping_class
    {
        private IEntityMapping Mapping { get; set; }

        [Test]
        public void Should_get_the_classes_collection()
        {
            Mapping.Classes.Should().BeEmpty();
        }

        [Test]
        public void Should_get_the_properties_collection()
        {
            Mapping.Properties.Should().BeEmpty();
        }

        [SetUp]
        public void Setup()
        {
            Mapping = new MergingEntityMapping(typeof(IProduct));
        }
    }
}
