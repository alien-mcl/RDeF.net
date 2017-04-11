using FluentAssertions;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;

namespace Given_instance_of.DefaultEntityContext_class
{
    [TestFixture]
    public class when_copying_entity : DefaultEntityContextTest
    {
        private static readonly Iri ExpectedIri = new Iri("copy");

        private IEntityContext SourceContext { get; set; }

        private IProduct Source { get; set; }

        private IProduct Result { get; set; }

        public override void TheTest()
        {
            Result = Context.Copy(Source, ExpectedIri);
        }

        [Test]
        public void Should_assign_new_Iri()
        {
            Result.Iri.Should().Be(ExpectedIri);
        }

        [Test]
        public void Should_copy_that_entity_into_new_context()
        {
            Result.Context.Should().Be(Context);
        }

        [Test]
        public void Should_leave_source_entity_untouched()
        {
            Source.Context.Should().Be(SourceContext);
        }

        [Test]
        public void Should_copy_description()
        {
            Result.Description.Should().Be(Source.Description);
        }

        [Test]
        public void Should_copy_name()
        {
            Result.Name.Should().Be(Source.Name);
        }

        [Test]
        public void Should_copy_ordinal()
        {
            Result.Ordinal.Should().Be(Source.Ordinal);
        }

        [Test]
        public void Should_copy_price()
        {
            Result.Price.Should().Be(Source.Price);
        }

        [Test]
        public void Should_copy_comments()
        {
            Result.Comments.Should().BeEquivalentTo(Source.Comments);
        }

        [Test]
        public void Should_copy_categories()
        {
            Result.Categories.Should().BeEquivalentTo(Source.Categories);
        }

        protected override void ScenarioSetup()
        {
            SourceContext = new DefaultEntityContext(EntitySource.Object, MappingsRepository.Object, ChangeDetector.Object);
            Source = SourceContext.Create<IProduct>(new Iri("source"));
        }
    }
}
