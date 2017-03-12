using System;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Entities;

namespace Given_instance_of.Statement_class
{
    [TestFixture]
    public class when_comparing
    {
        private static readonly Statement Statement = new Statement(new Iri("test"), new Iri("test"), "test", "test", new Iri("test"));

        [Test]
        public void Should_confirm_two_same_statements_are_equal()
        {
            Statement.Equals(Statement).Should().BeTrue();
        }

        [Test]
        public void Should_confirm_statement_and_instance_of_another_type_are_note_equal()
        {
            Statement.Equals(String.Empty).Should().BeFalse();
        }

        [Test]
        public void Should_confirm_two_statements_are_equal_when_all_their_properties_are_equal()
        {
            Statement.Equals(new Statement(new Iri("test"), new Iri("test"), "test", "test", new Iri("test"))).Should().BeTrue();
        }
    }
}
