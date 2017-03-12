using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RollerCaster;

namespace Given_instance_of.Entity_class
{
    [TestFixture]
    public class when_setting_a_property : EntityTest
    {
        private const string ExpectedName = "Product name";
        private const string ExpectedDescription = "Product description";

        public override void TheTest()
        {
            Entity.ActLike<IProduct>().Name = ExpectedName;
            Entity.ActLike<IProduct>().Description = ExpectedDescription;
        }

        [Test]
        public void Should_initialize_the_entity_only_once()
        {
            Context.Verify(instance => instance.Initialize(Entity), Times.Once);
        }

        [Test]
        public void Should_set_the_product_name_correctly()
        {
            Entity.ActLike<IProduct>().Name.Should().Be(ExpectedName);
        }

        [Test]
        public void Should_set_the_product_description_correctly()
        {
            Entity.ActLike<IProduct>().Description.Should().Be(ExpectedDescription);
        }

        protected override void ScenarioSetup()
        {
            Context.Setup(instance => instance.Initialize(It.IsAny<Entity>()));
        }
    }
}
