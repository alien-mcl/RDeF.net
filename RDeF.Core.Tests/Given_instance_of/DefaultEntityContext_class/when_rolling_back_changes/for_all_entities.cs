using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Given_instance_of.DefaultEntityContext_class.when_rolling_back_changes
{
    [TestFixture]
    public class for_all_entities : ScenarioTest
    {
        public override Task TheTest()
        {
            Context.Rollback();
            return Task.CompletedTask;
        }

        [Test]
        public void Should_cancel_primary_product_description_change()
        {
            PrimaryProduct.Description.Should().BeNull();
        }

        [Test]
        public void Should_cancel_primary_product_name_change()
        {
            PrimaryProduct.Name.Should().BeNull();
        }

        [Test]
        public void Should_cancel_primary_product_ordinal_change()
        {
            PrimaryProduct.Ordinal.Should().Be(0);
        }

        [Test]
        public void Should_cancel_primary_product_price_change()
        {
            PrimaryProduct.Price.Should().Be(0.0);
        }

        [Test]
        public void Should_clear_primary_product_categories()
        {
            PrimaryProduct.Categories.Should().BeEmpty();
        }

        [Test]
        public void Should_clear_primary_product_comments()
        {
            PrimaryProduct.Comments.Should().BeEmpty();
        }

        [Test]
        public void Should_cancel_secondary_product_description_change()
        {
            SecondaryProduct.Description.Should().BeNull();
        }
    }
}
