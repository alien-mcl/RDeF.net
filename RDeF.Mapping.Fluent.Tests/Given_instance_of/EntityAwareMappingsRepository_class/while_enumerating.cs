using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Mapping;

namespace Given_instance_of.EntityAwareMappingsRepository_class
{
    [TestFixture]
    public class while_enumerating : EntityAwareMappingsRepositoryTest
    {
        [Test]
        public void Should_enumerate_all_entity_mappings()
        {
            Repository.ToList().Should().HaveCount(5).And.Subject.All(instance => instance == EntityMapping.Object).Should().BeTrue();
        }

        protected override void ScenarioSetup()
        {
            ExplicitMappings.Setup(instance => instance.GetEnumerator(OwningEntity))
                .Returns(Enumerable.Range(0, 2).Select(_ => EntityMapping.Object).GetEnumerator());
            MappingRepository.As<IEnumerable<IEntityMapping>>().Setup(instance => instance.GetEnumerator())
                .Returns(Enumerable.Range(0, 3).Select(_ => EntityMapping.Object).GetEnumerator());
        }
    }
}
