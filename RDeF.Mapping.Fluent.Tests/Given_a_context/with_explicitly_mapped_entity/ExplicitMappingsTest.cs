using System.Threading.Tasks;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Mapping;
using RDeF.Mapping.Converters;
using RDeF.Mapping.Explicit;

namespace Given_a_context.with_explicitly_mapped_entity
{
    public abstract class ExplicitMappingsTest
    {
        protected IEntityContext Context { get; set; }

        public virtual Task TheTest()
        {
            return Task.CompletedTask;
        }

        [SetUp]
        public async Task Setup()
        {
            ScenarioSetup();
            await TheTest();
        }

        [TearDown]
        public void Teardown()
        {
            ScenarioTeardown();
        }

        protected virtual void ScenarioSetup()
        {
        }

        protected virtual void ScenarioTeardown()
        {
        }

        protected virtual void MapPrimaryEntity(IExplicitMappingsBuilder<IUnmappedProduct> entity)
        {
            entity
                .MappedTo(new Iri("class1"))
                .MappedTo(new Iri("class2"))
                .WithProperty(instance => instance.Description).MappedTo(new Iri("description")).WithValueConverter<StringConverter>()
                .WithProperty(instance => instance.Name).MappedTo(new Iri("name")).WithValueConverter<StringConverter>()
                .WithCollection(instance => instance.Categories).MappedTo(new Iri("categories")).StoredAs(CollectionStorageModel.Simple).WithValueConverter<StringConverter>();
        }

        protected virtual void MapSecondaryEntity(IExplicitMappingsBuilder<IUnmappedProduct> entity)
        {
            entity
                .MappedTo(new Iri("class1"))
                .MappedTo(new Iri("class2"))
                .WithProperty(instance => instance.Description).MappedTo(new Iri("description")).WithValueConverter<StringConverter>()
                .WithProperty(instance => instance.Name).MappedTo(new Iri("label")).WithValueConverter<StringConverter>()
                .WithCollection(instance => instance.Categories).MappedTo(new Iri("categories")).StoredAs(CollectionStorageModel.Simple).WithValueConverter<StringConverter>();
        }

        protected virtual void AlternativeMapSecondaryEntity(IExplicitMappingsBuilder<IUnmappedProduct> entity)
        {
            entity
                .MappedTo(new Iri("class1"))
                .MappedTo(new Iri("class2"))
                .WithProperty(instance => instance.Description).MappedTo(new Iri("description")).WithValueConverter<StringConverter>()
                .WithProperty(instance => instance.Name).MappedTo(new Iri("display-name")).WithValueConverter<StringConverter>()
                .WithCollection(instance => instance.Categories).MappedTo(new Iri("categories")).StoredAs(CollectionStorageModel.Simple).WithValueConverter<StringConverter>();
        }
    }
}
