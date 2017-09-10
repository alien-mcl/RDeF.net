using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Mapping;
using RDeF.Mapping.Providers;

namespace Given_instance_of.DefaultMappingsRepository_class.which_is_already_initialized
{
    [TestFixture]
    public class and_searching_for_type_mapping : DefaultMappingsRepositoryTest
    {
        private static readonly Type ExpectedType = typeof(IProduct);

        private IEntityMapping Result { get; set; }

        public override void TheTest()
        {
            Result = MappingsRepository.FindEntityMappingFor<IProduct>();
        }

        [Test]
        public void Should_retrieve_the_entity_mapping()
        {
            Result.Type.Should().Be(ExpectedType);
        }

        [Test]
        public void Should_throw_when_no_type_is_given()
        {
            MappingsRepository.Invoking(instance => instance.FindEntityMappingFor(null)).ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Should_throw_when_invalid_type_is_given()
        {
            MappingsRepository.Invoking(instance => instance.FindEntityMappingFor(typeof(string))).ShouldThrow<ArgumentOutOfRangeException>();
        }

        protected override void ScenarioSetup()
        {
            var entityMapping = new Mock<IEntityMapping>(MockBehavior.Strict);
            entityMapping.SetupGet(instance => instance.Type).Returns(typeof(IProduct));
            MappingBuilder.Setup(instance => instance.BuildMappings(It.IsAny<IEnumerable<IMappingSource>>(), It.IsAny<IDictionary<Type, ICollection<ITermMappingProvider>>>()))
                .Returns(new Dictionary<Type, IEntityMapping>() { { typeof(IProduct), entityMapping.Object } });
        }
    }
}
