using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Moq;
using NUnit.Framework;
using RDeF.Entities;
using RDeF.Mapping;
using RDeF.Mapping.Converters;
using RDeF.Mapping.Providers;
using RDeF.Mapping.Visitors;
using RollerCaster.Reflection;

namespace Given_instance_of.DefaultMappingBuilder_class
{
    public abstract class DefaultMappingBuilderTest
    {
        internal DefaultMappingBuilder Builder { get; set; }

        protected Mock<IMappingProviderVisitor> MappingProviderVisitor { get; private set; }

        protected Mock<IMappingSource> MappingSource { get; private set; }

        protected Mock<IConverterProvider> ConverterProvider { get; private set; }

        protected IDictionary<Type, ICollection<ITermMappingProvider>> OpenGenericMappingProviders { get; set; }

        public virtual void TheTest()
        {
        }

        [SetUp]
        public void Setup()
        {
            ConverterProvider = new Mock<IConverterProvider>(MockBehavior.Strict);
            MappingProviderVisitor = new Mock<IMappingProviderVisitor>(MockBehavior.Strict);
            MappingSource = new Mock<IMappingSource>(MockBehavior.Strict);
            Builder = new DefaultMappingBuilder(ConverterProvider.Object, Array.Empty<QIriMapping>(), new[] { MappingProviderVisitor.Object });
            ScenarioSetup();
            TheTest();
        }

        protected static IEnumerable<Mock<ITermMappingProvider>> SetupMappingProviders<T>(string @class, params string[] properties) where T : class, IEntity
        {
            return SetupMappingProviders(typeof(T), @class, properties);
        }

        protected static IEnumerable<Mock<ITermMappingProvider>> SetupMappingProviders(Type entityType, string @class, params string[] properties)
        {
            yield return SetupEntityMappingProvider(entityType, @class);
            foreach (var property in properties)
            {
                yield return SetupPropertyMapping(entityType, property);
            }
        }

        protected static Mock<ITermMappingProvider> SetupEntityMappingProvider(Type entityType, string @class)
        {
            var result = new Mock<IEntityMappingProvider>(MockBehavior.Strict);
            var termMappingProvider = result.As<ITermMappingProvider>();
            termMappingProvider.SetupGet(instance => instance.EntityType).Returns(entityType);
            termMappingProvider.Setup(instance => instance.GetTerm(It.IsAny<IEnumerable<QIriMapping>>())).Returns(new Iri(@class));
            termMappingProvider.Setup(instance => instance.GetGraph(It.IsAny<IEnumerable<QIriMapping>>())).Returns((Iri)null);
            termMappingProvider.Setup(instance => instance.Accept(It.IsAny<IMappingProviderVisitor>()));
            return termMappingProvider;
        }

        protected static Mock<ITermMappingProvider> SetupPropertyMapping(Type entityType, string name)
        {
            var allTypes = new[] { entityType }.Concat(entityType.GetTypeInfo().GetInterfaces().Where(@interface => typeof(IEntity).IsAssignableFrom(@interface)));
            var property = (from type in allTypes from item in type.GetProperties() where item.Name == name select item).First();
            Mock<IPropertyMappingProvider> propertyMappingProvider;
            if (property.PropertyType.IsAnEnumerable())
            {
                var collectionMappingProvider = new Mock<ICollectionMappingProvider>(MockBehavior.Strict);
                collectionMappingProvider.SetupGet(instance => instance.StoreAs).Returns(CollectionStorageModel.Simple);
                propertyMappingProvider = collectionMappingProvider.As<IPropertyMappingProvider>();
            }
            else
            {
                propertyMappingProvider = new Mock<IPropertyMappingProvider>(MockBehavior.Strict);
            }

            propertyMappingProvider.SetupGet(instance => instance.Property).Returns(property);
            propertyMappingProvider.SetupGet(instance => instance.ValueConverterType).Returns(typeof(TestConverter));
            var termMappingProvider = propertyMappingProvider.As<ITermMappingProvider>();
            termMappingProvider.SetupGet(instance => instance.EntityType).Returns(entityType);
            termMappingProvider.Setup(instance => instance.GetTerm(It.IsAny<IEnumerable<QIriMapping>>())).Returns(new Iri(name));
            termMappingProvider.Setup(instance => instance.GetGraph(It.IsAny<IEnumerable<QIriMapping>>())).Returns((Iri)null);
            termMappingProvider.Setup(instance => instance.Accept(It.IsAny<IMappingProviderVisitor>()));
            return termMappingProvider;
        }

        protected virtual void ScenarioSetup()
        {
        }
    }
}
