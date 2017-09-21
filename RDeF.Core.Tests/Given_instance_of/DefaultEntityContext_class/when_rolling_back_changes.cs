using System.Reflection;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Mapping;

namespace Given_instance_of.DefaultEntityContext_class
{
    [TestFixture]
    public class when_rolling_back_changes : DefaultEntityContextTest
    {
        private IProduct Product { get; set; }

        public override void TheTest()
        {
            Context.Rollback();
        }

        [Test]
        public void Should_cancel_description_change()
        {
            Product.Description.Should().BeNull();
        }

        [Test]
        public void Should_cancel_name_change()
        {
            Product.Name.Should().BeNull();
        }

        [Test]
        public void Should_cancel_ordinal_change()
        {
            Product.Ordinal.Should().Be(0);
        }

        [Test]
        public void Should_cancel_price_change()
        {
            Product.Price.Should().Be(0.0);
        }

        [Test]
        public void Should_clear_categories()
        {
            Product.Categories.Should().BeEmpty();
        }

        [Test]
        public void Should_clear_comments()
        {
            Product.Comments.Should().BeEmpty();
        }

        protected override void ScenarioSetup()
        {
            MappingsRepository.Setup(instance => instance.FindPropertyMappingFor(It.IsAny<IEntity>(), It.IsAny<PropertyInfo>()))
                .Returns<IEntity, PropertyInfo>((entity, propertyInfo) =>
                {
                    var result = new Mock<IPropertyMapping>(MockBehavior.Strict);
                    result.SetupGet(instance => instance.PropertyInfo).Returns(propertyInfo);
                    return result.Object;
                });
            Product = Context.Create<IProduct>(new Iri("test"));
            Product.Description = "Product description";
            Product.Name = "Product name";
            Product.Ordinal = 1;
            Product.Price = 3.14159;
            Product.Categories.Add("category 1");
            Product.Categories.Add("category 2");
            Product.Comments.Add("comment 1");
            Product.Comments.Add("comment 2");
        }
    }
}
