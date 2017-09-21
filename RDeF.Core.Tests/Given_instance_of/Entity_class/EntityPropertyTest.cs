using System.Reflection;
using Moq;
using RDeF.Entities;
using RDeF.Mapping;

namespace Given_instance_of.Entity_class
{
    public abstract class EntityPropertyTest : EntityTest
    {
        protected Mock<IMappingsRepository> MappingsRepository { get; private set; }

        protected override void ScenarioSetup()
        {
            MappingsRepository = new Mock<IMappingsRepository>(MockBehavior.Strict);
            MappingsRepository.Setup(instance => instance.FindPropertyMappingFor(It.IsAny<IEntity>(), It.IsAny<PropertyInfo>()))
                .Returns<IEntity, PropertyInfo>((entity, propertyInfo) =>
                {
                    var result = new Mock<IPropertyMapping>(MockBehavior.Strict);
                    result.SetupGet(instance => instance.PropertyInfo).Returns(propertyInfo);
                    return result.Object;
                });
            Context.SetupGet(instance => instance.Mappings).Returns(MappingsRepository.Object);
        }
    }
}
