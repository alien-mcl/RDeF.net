using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Vocabularies;

namespace Given_instance_of.DefaultEntityContext_class.when_rolling_back_changes
{
    [TestFixture]
    public class for_selected_entities : ScenarioTest
    {
        public override Task TheTest()
        {
            Context.Rollback(new[] { rdfs.Datatype });
            return Task.CompletedTask;
        }

        [Test]
        public void Should_not_cancel_primary_product_description_change()
        {
            PrimaryProduct.Description.Should().Be("Product description");
        }

        [Test]
        public void Should_not_cancel_primary_product_name_change()
        {
            PrimaryProduct.Name.Should().Be("Product name");
        }

        [Test]
        public void Should_not_cancel_primary_product_ordinal_change()
        {
            PrimaryProduct.Ordinal.Should().Be(1);
        }

        [Test]
        public void Should_not_cancel_primary_product_price_change()
        {
            PrimaryProduct.Price.Should().Be(3.14159);
        }

        [Test]
        public void Should_not_clear_primary_product_categories()
        {
            PrimaryProduct.Categories.Should().HaveCount(2);
        }

        [Test]
        public void Should_not_clear_primary_product_comments()
        {
            PrimaryProduct.Comments.Should().HaveCount(2);
        }
        
        [Test]
        public void Should_cancel_secondary_product_description_change()
        {
            SecondaryProduct.Description.Should().BeNull();
        }
    }
}
