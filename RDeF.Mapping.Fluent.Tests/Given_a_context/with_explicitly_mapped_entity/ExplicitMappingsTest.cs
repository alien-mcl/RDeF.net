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
                .MappedTo(new Iri("some:class1"))
                .MappedTo(new Iri("some:class2"))
                .WithProperty(instance => instance.Description).MappedTo(new Iri("some:description")).WithValueConverter<StringConverter>()
                .WithProperty(instance => instance.Name).MappedTo(new Iri("some:name")).WithValueConverter<StringConverter>()
                .WithCollection(instance => instance.Categories).MappedTo(new Iri("some:categories")).StoredAs(CollectionStorageModel.Simple).WithValueConverter<StringConverter>();
        }

        protected virtual void MapSecondaryEntity(IExplicitMappingsBuilder<IUnmappedProduct> entity)
        {
            entity
                .MappedTo(new Iri("some:class1"))
                .MappedTo(new Iri("some:class2"))
                .WithProperty(instance => instance.Description).MappedTo(new Iri("some:description")).WithValueConverter<StringConverter>()
                .WithProperty(instance => instance.Name).MappedTo(new Iri("some:label")).WithValueConverter<StringConverter>()
                .WithCollection(instance => instance.Categories).MappedTo(new Iri("some:categories")).StoredAs(CollectionStorageModel.Simple).WithValueConverter<StringConverter>();
        }

        protected virtual void AlternativeMapSecondaryEntity(IExplicitMappingsBuilder<IUnmappedProduct> entity)
        {
            entity
                .MappedTo(new Iri("some:class1"))
                .MappedTo(new Iri("some:class2"))
                .WithProperty(instance => instance.Description).MappedTo(new Iri("some:description")).WithValueConverter<StringConverter>()
                .WithProperty(instance => instance.Name).MappedTo(new Iri("some:display-name")).WithValueConverter<StringConverter>()
                .WithCollection(instance => instance.Categories).MappedTo(new Iri("some:categories")).StoredAs(CollectionStorageModel.Simple).WithValueConverter<StringConverter>();
        }
    }
}
