using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;

namespace Given_instance_of.DefaultEntityContext_class
{
    [TestFixture]
    public class when_clearing : DefaultEntityContextTest
    {
        public override void TheTest()
        {
            Context.Clear();
        }

        [Test]
        public void Should_delete_it_from_cache()
        {
            ((IDictionary<Iri, Entity>)Context.GetType().GetField("_entityCache", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Context))
                .Should().HaveCount(0);
        }

        protected override void ScenarioSetup()
        {
            Context.Create<IProduct>(new Iri(new Uri("http://test.com/")));
        }
    }
}
