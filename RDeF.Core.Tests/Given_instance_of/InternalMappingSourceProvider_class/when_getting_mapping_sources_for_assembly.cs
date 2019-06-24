using System.Linq;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Mapping;

namespace Given_instance_of.InternalMappingSourceProvider_class
{
    [TestFixture]
    public class when_getting_mapping_sources_for_assembly
    {
        private InternalMappingSourceProvider Provider { get; set; }

        [Test]
        public void Should_gather_all_mapping_sources_for_that_assembly()
        {
            Provider.GetMappingSourcesFor(typeof(ITypedEntity).GetTypeInfo().Assembly).Should().HaveCount(1).And.Subject.First().Should().BeOfType<InternalMappingSource>();
        }

        [Test]
        public void Should_not_gather_any_mapping_sources_from_outside_Contracts_assembly()
        {
            Provider.GetMappingSourcesFor(typeof(IProduct).GetTypeInfo().Assembly).Should().BeEmpty();
        }

        [SetUp]
        public void Setup()
        {
            Provider = new InternalMappingSourceProvider();
        }
    }
}
