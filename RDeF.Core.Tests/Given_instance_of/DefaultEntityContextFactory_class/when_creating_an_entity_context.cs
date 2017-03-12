using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using RDeF.ComponentModel;
using RDeF.Data;
using RDeF.Entities;

namespace Given_instance_of.DefaultEntityContextFactory_class
{
    public class when_creating_an_entity_context : DefaultEntityContextFactoryTest
    {
        private static readonly IActivator Activator = new DefaultActivator();
        private static readonly IEntitySource EntitySource = new SimpleInMemoryEntitySource();

        private IEntityContext Result { get; set; }

        public override void TheTest()
        {
            Result = Factory
                .WithActivator<DefaultActivator>()
                .WithActivator<DefaultActivator>()
                .WithEntitySource<SimpleInMemoryEntitySource>()
                .WithEntitySource<SimpleInMemoryEntitySource>()
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
            Result.MappingsRepository.Should().HaveCount(4).And.Subject.First(entity => entity.Type == typeof(IProduct)).Properties.Should().HaveCount(4);
        }

        [Test]
        public void Should_throw_when_activator_type_is_invalid()
        {
            Factory.Invoking(instance => instance.WithActivator<string>()).ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void Should_throw_when_entity_source_type_is_invalid()
        {
            Factory.Invoking(instance => instance.WithEntitySource<string>()).ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void Should_throw_when_mappings_builder_is_not_given()
        {
            Factory.Invoking(instance => instance.WithMappings(null)).ShouldThrow<ArgumentNullException>();
        }

        protected override void ScenarioSetup()
        {
            Factory
                .WithActivator(Activator)
                .WithActivator(Activator)
                .WithActivator<DefaultActivator>()
                .WithActivator<DefaultActivator>()
                .WithActivator(Activator)
                .WithActivator<DefaultActivator>()
                .WithEntitySource(EntitySource)
                .WithEntitySource(EntitySource)
                .WithEntitySource<SimpleInMemoryEntitySource>()
                .WithEntitySource<SimpleInMemoryEntitySource>()
                .WithEntitySource(EntitySource)
                .WithEntitySource<SimpleInMemoryEntitySource>()
                .WithMappings(_ => _.FromAssemblyOf<IProduct>())
                .Create();
        }
    }
}
