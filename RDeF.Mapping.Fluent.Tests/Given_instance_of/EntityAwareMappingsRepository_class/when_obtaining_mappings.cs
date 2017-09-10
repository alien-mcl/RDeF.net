using System;
using System.Reflection;
using Moq;
using NUnit.Framework;
using RDeF.Entities;

namespace Given_instance_of.EntityAwareMappingsRepository_class
{
    [TestFixture]
    public class when_obtaining_mappings : EntityAwareMappingsRepositoryTest
    {
        [Test]
        public void Should_obtain_entity_mapping_from_underlying_repository()
        {
            Configuring(MappingRepository).Member(instance => instance.FindEntityMappingFor(It.IsAny<Iri>(), It.IsAny<Iri>())).ToReturn(EntityMapping.Object)
                .Calling(repository => repository.FindEntityMappingFor(Class))
                .ShouldCallConfiguredMemberWith(Class, null).ResultingWithExpected(EntityMapping.Object);
        }

        [Test]
        public void Should_obtain_entity_mapping_from_explicit_mappings()
        {
            Configuring(ExplicitMappings).Member(instance => instance.FindEntityMappingFor(It.IsAny<Type>(), It.IsAny<Iri>())).ToReturn(EntityMapping.Object)
                .Calling(repository => repository.FindEntityMappingFor(typeof(IEntity)))
                .ShouldCallConfiguredMemberWith(typeof(IEntity), OwningEntity).ResultingWithExpected(EntityMapping.Object);
        }

        [Test]
        public void Should_obtain_entity_mapping_from_underlying_repository_when_explicit_mappings_are_not_defined()
        {
            Configuring(ExplicitMappings).Member(instance => instance.FindEntityMappingFor(It.IsAny<Type>(), It.IsAny<Iri>())).ToReturn(null)
                .And
                .Configuring(MappingRepository).Member(instance => instance.FindEntityMappingFor(It.IsAny<Type>())).ToReturn(EntityMapping.Object)
                .Calling(repository => repository.FindEntityMappingFor<IEntity>())
                .ShouldCallConfiguredMemberWith(typeof(IEntity)).ResultingWithExpected(EntityMapping.Object);
        }

        [Test]
        public void Should_obtain_property_mapping_from_underlying_repository()
        {
            Configuring(MappingRepository).Member(instance => instance.FindPropertyMappingFor(It.IsAny<Iri>(), It.IsAny<Iri>())).ToReturn(PropertyMapping.Object)
                .Calling(repository => repository.FindPropertyMappingFor(Predicate))
                .ShouldCallConfiguredMemberWith(Predicate, null).ResultingWithExpected(PropertyMapping.Object);
        }

        [Test]
        public void Should_obtain_property_mapping_from_explicit_mappings()
        {
            Configuring(ExplicitMappings).Member(instance => instance.FindPropertyMappingFor(It.IsAny<PropertyInfo>(), It.IsAny<Iri>())).ToReturn(PropertyMapping.Object)
                .Calling(repository => repository.FindPropertyMappingFor(typeof(IEntity).GetProperty("Id")))
                .ShouldCallConfiguredMemberWith(typeof(IEntity).GetProperty("Id"), OwningEntity).ResultingWithExpected(PropertyMapping.Object);
        }

        [Test]
        public void Should_obtain_property_mapping_from_underlying_repository_when_explicit_mappings_are_not_defined()
        {
            Configuring(ExplicitMappings).Member(instance => instance.FindPropertyMappingFor(It.IsAny<PropertyInfo>(), It.IsAny<Iri>())).ToReturn(null)
                .And
                .Configuring(MappingRepository).Member(instance => instance.FindPropertyMappingFor(It.IsAny<PropertyInfo>())).ToReturn(PropertyMapping.Object)
                .Calling(repository => repository.FindPropertyMappingFor(typeof(IEntity).GetProperty("Id")))
                .ShouldCallConfiguredMemberWith(typeof(IEntity).GetProperty("Id")).ResultingWithExpected(PropertyMapping.Object);
        }
    }
}
