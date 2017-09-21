using Moq;
using NUnit.Framework;
using RDeF.Entities;

namespace Given_instance_of.DefaultEntityContext_class
{
    [TestFixture]
    public class and_all_properties_are_mapped : ScenarioTest
    {
        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();
            MappingsRepository.Setup(instance => instance.FindPropertyMappingFor(It.IsAny<IEntity>(), It.IsAny<Iri>(), It.IsAny<Iri>()))
                .Returns<IEntity, Iri, Iri>((entity, iri, graph) => PropertyMapping.Object);
        }
    }
}
