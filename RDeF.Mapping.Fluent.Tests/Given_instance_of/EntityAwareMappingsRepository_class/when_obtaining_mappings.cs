using System;
using System.Reflection;
using Moq;
using NUnit.Framework;
using RDeF.Entities;
using RDeF.Mapping;

namespace Given_instance_of.EntityAwareMappingsRepository_class
{
    [TestFixture]
    public class when_obtaining_mappings : EntityAwareMappingsRepositoryTest
    {
        [Test]
        public void Should_obtain_entity_mapping_from_underlying_repository()
        {
            Configuring(MappingRepository).Member(_ => _.FindEntityMappingFor(It.IsAny<IEntity>(), It.IsAny<Iri>(), It.IsAny<Iri>()))
                .ToReturn(EntityMapping.Object)
                .Calling(that => that.FindEntityMappingFor(OwningEntity.Object, Class))
                .ShouldCallConfiguredMemberWith(OwningEntity.Object, Class, null).ResultingWithExpected(EntityMapping.Object);
        }

        [Test]
        public void Should_obtain_entity_mapping_from_explicit_mappings()
        {
            Configuring(ExplicitMappings).Member(_ => _.FindEntityMappingFor(It.IsAny<Type>(), It.IsAny<Iri>()))
                .ToReturn(EntityMapping.Object)
                .Calling(that => that.FindEntityMappingFor(OwningEntity.Object, typeof(IEntity)))
                .ShouldCallConfiguredMemberWith(typeof(IEntity), Iri).ResultingWithExpected(EntityMapping.Object);
        }

        [Test]
        public void Should_obtain_entity_mapping_from_underlying_repository_when_explicit_mappings_are_not_defined()
        {
            Configuring(ExplicitMappings).Member(instance => instance.FindEntityMappingFor(It.IsAny<Type>(), It.IsAny<Iri>()))
                .ToReturn(null)
                .And
                .Configuring(MappingRepository).Member(_ => _.FindEntityMappingFor(It.IsAny<IEntity>(), It.IsAny<Type>())).ToReturn(EntityMapping.Object)
                .Calling(that => that.FindEntityMappingFor(OwningEntity.Object, typeof(IEntity)))
                .ShouldCallConfiguredMemberWith(OwningEntity.Object, typeof(IEntity)).ResultingWithExpected(EntityMapping.Object);
        }

        [Test]
        public void Should_obtain_term_property_mapping_from_explicit_mappings()
        {
            Configuring(ExplicitMappings).Member(instance => instance.FindPropertyMappingsFor(It.IsAny<Iri>(), It.IsAny<Iri>(), It.IsAny<Iri>())).
                ToReturn(new[] { PropertyMapping.Object })
                .Calling(that => that.FindPropertyMappingsFor(OwningEntity.Object, Predicate))
                .ShouldCallConfiguredMemberWith(Predicate, null, Iri).ResultingWithExpected(new[] { PropertyMapping.Object });
        }

        [Test]
        public void Should_obtain_term_property_mapping_from_underlying_repository_when_explicit_mappings_are_not_defined()
        {
            Configuring(ExplicitMappings).Member(instance => instance.FindPropertyMappingsFor(It.IsAny<Iri>(), It.IsAny<Iri>(), It.IsAny<Iri>()))
                .ToReturn(Array.Empty<IPropertyMapping>())
                .And
                .Configuring(MappingRepository).Member(_ => _.FindPropertyMappingsFor(It.IsAny<IEntity>(), It.IsAny<Iri>(), It.IsAny<Iri>()))
                .ToReturn(new[] { PropertyMapping.Object })
                .Calling(that => that.FindPropertyMappingsFor(OwningEntity.Object, Predicate))
                .ShouldCallConfiguredMemberWith(OwningEntity.Object, Predicate, null).ResultingWithExpected(new[] { PropertyMapping.Object });
        }

        [Test]
        public void Should_obtain_property_mapping_from_explicit_mappings()
        {
            Configuring(ExplicitMappings).Member(_ => _.FindPropertyMappingFor(It.IsAny<PropertyInfo>(), It.IsAny<Iri>()))
                .ToReturn(PropertyMapping.Object)
                .Calling(that => that.FindPropertyMappingFor(OwningEntity.Object, typeof(IEntity).GetProperty("Id")))
                .ShouldCallConfiguredMemberWith(typeof(IEntity).GetProperty("Id"), Iri).ResultingWithExpected(PropertyMapping.Object);
        }

        [Test]
        public void Should_obtain_property_mapping_from_underlying_repository_when_explicit_mappings_are_not_defined()
        {
            Configuring(ExplicitMappings).Member(_ => _.FindPropertyMappingFor(It.IsAny<PropertyInfo>(), It.IsAny<Iri>()))
                .ToReturn(null)
                .And
                .Configuring(MappingRepository).Member(instance => instance.FindPropertyMappingFor(It.IsAny<IEntity>(), It.IsAny<PropertyInfo>()))
                .ToReturn(PropertyMapping.Object)
                .Calling(that => that.FindPropertyMappingFor(OwningEntity.Object, typeof(IEntity).GetProperty("Id")))
                .ShouldCallConfiguredMemberWith(OwningEntity.Object, typeof(IEntity).GetProperty("Id")).ResultingWithExpected(PropertyMapping.Object);
        }
    }
}
