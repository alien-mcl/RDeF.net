using System.Globalization;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Entities;
using RDeF.Mapping.Converters;

namespace Given_instance_of.converter_of_type
{
    [TestFixture]
    public abstract class LiteralConverterTest<TConverter> where TConverter : LiteralConverterBase, new()
    {
        protected static readonly Iri Subject = new Iri("subject");
        protected static readonly Iri Predicate = new Iri("predicate");

        protected TConverter Converter { get; private set; }

        private CultureInfo CurrentCulture { get; set; }

        private CultureInfo CurrentUICulture { get; set; }

        [Test]
        public void Should_confirm_two_converters_of_same_types_are_equal()
        {
            Converter.Equals(new TConverter()).Should().BeTrue();
        }

        [Test]
        public void Should_confirm_two_converters_of_different_types_are_unequal()
        {
            Converter.Equals(new TestConverter()).Should().BeFalse();
        }

        [SetUp]
        public void Setup()
        {
            CurrentCulture = CultureInfo.CurrentCulture;
            CurrentUICulture = CultureInfo.CurrentUICulture;
            Converter = new TConverter();
        }

        [TearDown]
        public void Teardown()
        {
            CultureInfo.CurrentCulture = CurrentCulture;
            CultureInfo.CurrentUICulture = CurrentUICulture;
        }

        protected Statement StatementFor(string value, Iri dataType = null)
        {
            return new Statement(Subject, Predicate, value, dataType);
        }
    }
}