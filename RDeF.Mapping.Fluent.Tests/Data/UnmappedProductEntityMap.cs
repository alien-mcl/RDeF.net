using RDeF.Data;
using RDeF.Entities;
using RDeF.Mapping.Converters;
using RDeF.Mapping.Fluent;

namespace RDeF.Mapping.Data
{
    public class UnmappedProductEntityMap : EntityMap<IUnmappedProduct>
    {
        public override void CreateMappings()
        {
            this.MappedTo(new Iri("class1"))
                .MappedTo("prefix", "term", null, null)
                .MappedTo("prefix", "term", new Iri("graph"))
                .WithProperty(instance => instance.Name).MappedTo(new Iri("name")).WithDefaultConverter()
                .WithProperty(instance => instance.Description).MappedTo("prefix", "description", null, null).WithValueConverter<TestConverter>()
                .WithProperty(instance => instance.Image).MappedTo("prefix", "image", new Iri("graph")).WithValueConverter<TestConverter>()
                .WithCollection(instance => instance.Categories).MappedTo(new Iri("categories")).StoredAs(CollectionStorageModel.Simple).WithDefaultConverter()
                .WithCollection(instance => instance.Comments).MappedTo("prefix", "comments", new Iri("graph")).StoredAs(CollectionStorageModel.LinkedList).WithValueConverter<TestConverter>()
                .WithCollection(instance => instance.Ordinals).MappedTo("prefix", "ordinals", null, null).StoredAs(CollectionStorageModel.LinkedList).WithValueConverter<TestConverter>();
        }
    }
}
