using FluentAssertions;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;

namespace Given_instance_of.DefaultEntityContextFactory_class
{
    [TestFixture]
    public class when_created_from_configuration
    {
        [Test]
        public void Should_include_pointed_mapping_assemblies()
        {
            EntityContextFactory.FromConfiguration("test").Mappings.FindEntityMappingFor<IProduct>(null).Should().NotBeNull();
        }
    }
}
