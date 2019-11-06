using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Collections;
using RDeF.Entities;
using RDeF.FluentAssertions;
using RDeF.Vocabularies;

namespace Given_instance_of.TypePrioritizingStatementCollection_class
{
    [TestFixture]
    public class when_operating
    {
        private Statement[] Statements { get; set; }

        private Statement Predicate { get; set; }

        private Statement Typing { get; set; }

        private TypePrioritizingStatementCollection Collection { get; set; }

        [Test]
        public void Should_provide_correct_number_of_items()
        {
            Collection.Count.Should().Be(2);
        }

        [Test]
        public void Should_be_writable()
        {
            Collection.IsReadOnly.Should().BeFalse();
        }

        [Test]
        public void Should_get_cleared()
        {
            Collection.After(_ => _.Clear()).Count.Should().Be(0);
        }

        [Test]
        public void Should_confirm_an_item_contained()
        {
            Collection.Contains(Predicate).Should().BeTrue();
        }

        [Test]
        public void Should_copy_to_array()
        {
            Statements.After(_ => ((ICollection<Statement>)Collection).CopyTo(Statements, 0))
                .Should().BeEquivalentTo(Typing, Predicate);
        }

        [Test]
        public void Should_remove_an_existing_statement()
        {
            Collection.After(_ => _.Remove(Predicate)).Count.Should().Be(1);
        }

        [Test]
        public void Should_put_predicate_statement_last()
        {
            Collection.Last().Should().Be(Predicate);
        }

        [Test]
        public void Should_put_typing_statement_first()
        {
            ((IEnumerable)Collection).GetEnumerator().After(_ => _.MoveNext()).Current.Should().Be(Typing);
        }

        [SetUp]
        public void Setup()
        {
            Collection = new TypePrioritizingStatementCollection();
            Collection.Add(Predicate = new Statement(new Iri("subject"), new Iri("predicate"), new Iri("object")));
            Collection.Add(Typing = new Statement(new Iri("subject"), rdf.type, new Iri("object")));
            Statements = new Statement[2];
        }
    }
}
