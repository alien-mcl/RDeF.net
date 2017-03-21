using System;
using RDeF.Mapping;
using RDeF.Mapping.Visitors;

namespace Given_instance_of.DefaultMappingRepository_class.which_is_already_initialized
{
    public class ScenarioTest : DefaultMappingRepositoryTest
    {
        protected override void ScenarioSetup()
        {
            MappingRepository = new DefaultMappingRepository(
                new[] { MappingSource.Object },
                Array.Empty<IMappingProviderVisitor>(),
                new[] { new TestConverter() },
                Array.Empty<QIriMapping>());
        }
    }
}
