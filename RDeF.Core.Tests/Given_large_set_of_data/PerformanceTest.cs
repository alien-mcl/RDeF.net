using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;

namespace Given_large_set_of_data
{
    public class PerformanceTest
    {
        protected const int MaxEntities = 1000;

        protected IEntityContext Context { get; set; }

        public virtual void TheTest()
        {
        }

        [SetUp]
        public void Setup()
        {
            Context = new DefaultEntityContextFactory()
                .WithMappings(gathered => gathered.FromAssemblyOf<IProduct>())
                .Create();
            InitializeEntities();
            Context.Commit();
            ScenarioSetup();
            TheTest();
        }

        [TearDown]
        public void Teardown()
        {
            Context.Dispose();
        }

        protected virtual void ScenarioSetup()
        {
        }

        private void InitializeEntities()
        {
            for (var index = 0; index < MaxEntities; index++)
            {
                var product = Context.Create<IProduct>(new Iri("product" + index));
                product.Name = "Product " + index;
                product.Price = index;
                product.Ordinal = index;
                product.Categories.Add("category 1-" + index);
                product.Categories.Add("category 2-" + index);
                product.Comments.Add("comment 1-" + index);
                product.Comments.Add("comment 2-" + index);
            }
        }
    }
}
