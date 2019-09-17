﻿using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Entities;
using RDeF.Mapping.Entities;

namespace Given_a_context.with_explicitly_mapped_entity
{
    [TestFixture]
    public class when_disposing : ExplicitMappingsTest
    {
        public override Task TheTest()
        {
            Context.Dispose();
            return Task.CompletedTask;
        }

        [Test]
        public void Should_serialize_explicitly_mapped_properties()
        {
            EntityContextExtensions.ExplicitMappings.Should().NotContainKey(Context);
        }

        protected override void ScenarioSetup()
        {
            Context = new DefaultEntityContextFactory().Create();
            Context.Commit();
        }
    }
}
