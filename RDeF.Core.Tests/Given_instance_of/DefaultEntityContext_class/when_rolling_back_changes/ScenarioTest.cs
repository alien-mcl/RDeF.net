using System.Reflection;
using Moq;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Mapping;
using RDeF.Vocabularies;

namespace Given_instance_of.DefaultEntityContext_class.when_rolling_back_changes
{
    public abstract class ScenarioTest : DefaultEntityContextTest
    {
        protected IProduct PrimaryProduct { get; private set; }

        protected IProduct SecondaryProduct { get; private set; }

        protected override void ScenarioSetup()
        {
            MappingsRepository.Setup(instance => instance.FindPropertyMappingFor(It.IsAny<IEntity>(), It.IsAny<PropertyInfo>()))
                .Returns<IEntity, PropertyInfo>((entity, propertyInfo) =>
                {
                    var result = new Mock<IPropertyMapping>(MockBehavior.Strict);
                    result.SetupGet(instance => instance.PropertyInfo).Returns(propertyInfo);
                    return result.Object;
                });
            PrimaryProduct = Context.Create<IProduct>(rdfs.Class);
            PrimaryProduct.Description = "Product description";
            PrimaryProduct.Name = "Product name";
            PrimaryProduct.Ordinal = 1;
            PrimaryProduct.Price = 3.14159;
            PrimaryProduct.Categories.Add("category 1");
            PrimaryProduct.Categories.Add("category 2");
            PrimaryProduct.Comments.Add("comment 1");
            PrimaryProduct.Comments.Add("comment 2");
            SecondaryProduct = Context.Create<IProduct>(rdfs.Datatype);
            SecondaryProduct.Description = "Some description";
        }
    }
}
