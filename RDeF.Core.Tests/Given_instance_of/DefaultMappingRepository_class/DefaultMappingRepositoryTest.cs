using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Mapping;
using RDeF.Mapping.Providers;
using RDeF.Mapping.Visitors;

namespace Given_instance_of.DefaultMappingRepository_class
{
    public abstract class DefaultMappingRepositoryTest
    {
        protected DefaultMappingRepository MappingRepository { get; set; }

        protected Mock<IMappingProviderVisitor> MappingProviderVisitor { get; private set; }

        protected Mock<IMappingSource> MappingSource { get; private set; }

        public virtual void TheTest()
        {
        }

        [SetUp]
        public void Setup()
        {
            MappingProviderVisitor = new Mock<IMappingProviderVisitor>(MockBehavior.Strict);
            MappingSource = new Mock<IMappingSource>(MockBehavior.Strict);
            ScenarioSetup();
            TheTest();
        }

        protected static IEnumerable<Mock<ITermMappingProvider>> SetupMappingProviders(string @class, params string[] properties)
        {
            yield return SetupEntityMappingProvider(@class);
            foreach (var property in properties)
            {
                yield return SetupPropertyMapping(property);
            }
        }

        protected static Mock<ITermMappingProvider> SetupEntityMappingProvider(string @class)
        {
            var result = new Mock<IEntityMappingProvider>(MockBehavior.Strict);
            var termMappingProvider = result.As<ITermMappingProvider>();
            termMappingProvider.SetupGet(instance => instance.EntityType).Returns(typeof(IProduct));
            termMappingProvider.Setup(instance => instance.GetTerm(It.IsAny<IEnumerable<QIriMapping>>())).Returns(new Iri(@class));
            termMappingProvider.Setup(instance => instance.GetGraph(It.IsAny<IEnumerable<QIriMapping>>())).Returns((Iri)null);
            return termMappingProvider;
        }

        protected static Mock<ITermMappingProvider> SetupPropertyMapping(string name)
        {
            var result = new Mock<IPropertyMappingProvider>(MockBehavior.Strict);
            result.SetupGet(instance => instance.Property).Returns(typeof(IProduct).GetProperty(name));
            result.SetupGet(instance => instance.ValueConverterType).Returns(typeof(TestConverter));
            var termMappingProvider = result.As<ITermMappingProvider>();
            termMappingProvider.SetupGet(instance => instance.EntityType).Returns(typeof(IProduct));
            termMappingProvider.As<ITermMappingProvider>().Setup(instance => instance.GetTerm(It.IsAny<IEnumerable<QIriMapping>>())).Returns(new Iri(name));
            termMappingProvider.As<ITermMappingProvider>().Setup(instance => instance.GetGraph(It.IsAny<IEnumerable<QIriMapping>>())).Returns((Iri)null);
            return termMappingProvider;
        }

        protected virtual void ScenarioSetup()
        {
        }
    }
}
