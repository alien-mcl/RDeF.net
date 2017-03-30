using System;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.ComponentModel;
using RDeF.Data;
using RDeF.Entities;

namespace Given_instance_of.DefaultEntityContextFactory_class
{
    public class when_creating_an_entity_context : DefaultEntityContextFactoryTest
    {
        private Mock<IEntitySource> EntitySource { get; set; }

        private IEntityContext Result { get; set; }

        public override void TheTest()
        {
            Result = Factory
                .WithEntitySource(EntitySource.Object)
                .WithModule<TestModule>()
                .WithMappings(_ => _.FromAssemblyOf<IProduct>())
                .Create();
        }

        [Test]
        public void Should_create_an_entity_context_correctly()
        {
            Result.Should().BeOfType<DefaultEntityContext>();
        }

        [Test]
        public void Should_build_the_mappings_correctly()
        {
            Result.MappingsRepository.Should().HaveCount(5).And.Subject.First(entity => entity.Type == typeof(IProduct)).Properties.Should().HaveCount(6);
        }

        [Test]
        public void Should_throw_when_mappings_builder_is_not_given()
        {
            Factory.Invoking(instance => instance.WithMappings(null)).ShouldThrow<ArgumentNullException>();
        }

        protected override void ScenarioSetup()
        {
            EntitySource = new Mock<IEntitySource>(MockBehavior.Strict);
            Factory
                .WithEntitySource(EntitySource.Object)
                .WithEntitySource(EntitySource.Object)
                .WithEntitySource<SimpleInMemoryEntitySource>()
                .WithEntitySource<SimpleInMemoryEntitySource>()
                .WithEntitySource(EntitySource.Object)
                .WithEntitySource<SimpleInMemoryEntitySource>()
                .WithMappings(_ => _.FromAssemblyOf<IProduct>())
                .WithModule<TestModule>()
                .Create();
        }
    }
}
