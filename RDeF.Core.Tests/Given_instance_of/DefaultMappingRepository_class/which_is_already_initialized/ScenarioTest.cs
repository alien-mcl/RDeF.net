using System;
using RDeF.Mapping;

namespace Given_instance_of.DefaultMappingRepository_class.which_is_already_initialized
{
    public class ScenarioTest : DefaultMappingRepositoryTest
    {
        protected override void ScenarioSetup()
        {
            MappingRepository = new DefaultMappingRepository(new[] { MappingSource.Object }, new[] { new TestConverter() }, Array.Empty<QIriMapping>());
        }
    }
}
