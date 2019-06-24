using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Mapping.Providers;

namespace Given_instance_of.InternalMappingSource_class
{
    [TestFixture]
    public class when_gathering_entity_mappings : InternalMappingSourceTest
    {
        private IEnumerable<ITermMappingProvider> Result { get; set; }

        public override void TheTest()
        {
            Result = Source.GatherEntityMappingProviders();
        }

        [Test]
        public void Should_gather_all_Contracts_assembly_entity_mappings()
        {
            Result.Should().HaveCount(1);
        }

        [Test]
        public void Should_gather_all_property_mappings_for_an_entity()
        {
            Result.OfType<IPropertyMappingProvider>().Should().HaveCount(1).And.Subject.First().Should().BeOfType<InternalCollectionMappingProvider>();
        }
    }
}
