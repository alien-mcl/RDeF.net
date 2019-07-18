using System.Reflection;
using Moq;
using RDeF.Entities;
using RDeF.Mapping;

namespace Given_instance_of.Entity_class
{
    public abstract class EntityPropertyTest : EntityTest
    {
        protected override void ScenarioSetup()
        {
            MappingsRepository.Setup(instance => instance.FindPropertyMappingFor(It.IsAny<IEntity>(), It.IsAny<PropertyInfo>()))
                .Returns<IEntity, PropertyInfo>((entity, propertyInfo) =>
                {
                    var result = new Mock<IPropertyMapping>(MockBehavior.Strict);
                    result.SetupGet(instance => instance.PropertyInfo).Returns(propertyInfo);
                    return result.Object;
                });
        }
    }
}
