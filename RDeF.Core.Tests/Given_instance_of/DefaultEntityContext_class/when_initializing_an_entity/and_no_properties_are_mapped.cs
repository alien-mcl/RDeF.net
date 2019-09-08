using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Entities;
using RDeF.Mapping;

namespace Given_instance_of.DefaultEntityContext_class
{
    [TestFixture]
    public class and_no_properties_are_mapped : ScenarioTest
    {
        private int Calls { get; set; }

        [Test]
        public void Should_raise_event_for_unmapped_property()
        {
            Calls.Should().Be(1);
        }

        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();
            Calls = 0;
            Context.UnmappedPropertyEncountered += (sender, e) =>
            {
                if (Calls++ == 0)
                {
                    e.PropertyMapping = PropertyMapping.Object;
                }
            };
            MappingsRepository.Setup(instance => instance.FindPropertyMappingsFor(It.IsAny<IEntity>(), It.IsAny<Iri>(), It.IsAny<Iri>()))
                .Returns(Array.Empty<IPropertyMapping>());
        }
    }
}
