using System.Reflection;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RollerCaster;

namespace Given_instance_of.Entity_class
{
    [TestFixture]
    public class when_getting_a_property : EntityPropertyTest
    {
        private const string ExpectedName = "Product name";
        private const string ExpectedDescription = "Product description";

        private string ResultingName { get; set; }

        private string ResultingDescription { get; set; }

        private IProduct Product { get; set; }

        public override void TheTest()
        {
            ResultingName = Product.Name;
            ResultingDescription = Product.Description;
        }

        [Test]
        public void Should_initialize_the_entity_only_once()
        {
            Context.Verify(instance => instance.Initialize(Entity), Times.Once);
        }

        [Test]
        public void Should_get_the_product_name_correctly()
        {
            ResultingName.Should().Be(ExpectedName);
        }

        [Test]
        public void Should_get_the_product_description_correctly()
        {
            ResultingDescription.Should().Be(ExpectedDescription);
        }

        [Test]
        public void Should_obtain_mappings_repository()
        {
            Context.VerifyGet(instance => instance.Mappings, Times.Exactly(2));
        }

        [Test]
        public void Should_obtain_name_property_mapping()
        {
            MappingsRepository.Verify(instance => instance.FindPropertyMappingFor(Product, typeof(IProduct).GetTypeInfo().GetProperty("Name")), Times.Once);
        }

        [Test]
        public void Should_obtain_description_property_mapping()
        {
            MappingsRepository.Verify(instance => instance.FindPropertyMappingFor(Product, typeof(IProduct).GetTypeInfo().GetProperty("Description")), Times.Once);
        }

        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();
            Context.Setup(instance => instance.Initialize(It.IsAny<Entity>()))
                .Callback<Entity>(entity =>
                {
                    entity.SetPropertyInternal(typeof(IProduct).GetTypeInfo().GetProperty("Name"), ExpectedName);
                    entity.SetPropertyInternal(typeof(IProduct).GetTypeInfo().GetProperty("Description"), ExpectedDescription);
                });
            Product = Entity.ActLike<IProduct>();
        }
    }
}
