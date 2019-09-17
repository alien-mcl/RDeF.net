using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;

namespace Given_instance_of.DefaultEntityContext_class
{
    public class when_deleting_an_entity : DefaultEntityContextTest
    {
        private static readonly Iri Iri = new Iri(new Uri("http://test.com/"));

        public override Task TheTest()
        {
            Context.Delete(Iri);
            return Task.CompletedTask;
        }

        [Test]
        public void Should_throw_when_no_Iri_is_given()
        {
            Context.Invoking(context => context.Delete(null)).ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Should_throw_when_given_Iri_is_a_blank()
        {
            Context.Invoking(context => context.Delete(new Iri())).ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Should_delete_it_from_cache()
        {
            ((IDictionary<Iri, Entity>)Context.GetType().GetField("_entityCache", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Context))
                .Should().NotContainKey(Iri);
        }

        [Test]
        public void Should_mark_it_for_deletion()
        {
            ((ICollection<Iri>)Context.GetType().GetField("_deletedEntities", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Context))
                .Should().Contain(Iri);
        }

        protected override void ScenarioSetup()
        {
            Context.Create<IProduct>(Iri);
        }
    }
}
