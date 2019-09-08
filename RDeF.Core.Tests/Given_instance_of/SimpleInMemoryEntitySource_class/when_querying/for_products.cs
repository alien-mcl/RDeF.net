using System;
using System.Collections.Generic;
using System.Reflection;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Mapping;

namespace Given_instance_of.SimpleInMemoryEntitySource_class.when_querying
{
    [TestFixture]
    public class for_products : ScenarioTest<IProduct>
    {
        private Mock<IEntityMapping> ProductMapping { get; set; }

        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();
            var statementMapping = new Mock<IStatementMapping>(MockBehavior.Strict);
            statementMapping.SetupGet(_ => _.Term).Returns(new Iri("http://temp.uri/Product"));
            ProductMapping = new Mock<IEntityMapping>(MockBehavior.Strict);
            ProductMapping.SetupGet(_ => _.Type).Returns(typeof(IProduct));
            ProductMapping.SetupGet(_ => _.Classes).Returns(new[] { statementMapping.Object });
            MappingsRepository.As<IEnumerable<IEntityMapping>>()
                .Setup(_ => _.GetEnumerator()).Returns(new List<IEntityMapping>() { ProductMapping.Object }.GetEnumerator());
            MappingsRepository.Setup(_ => _.FindEntityMappingFor(It.IsAny<IEntity>(), It.IsAny<Type>()))
                .Returns(ProductMapping.Object);
            var propertyMapping = new Mock<IPropertyMapping>(MockBehavior.Strict);
            propertyMapping.SetupGet(_ => _.PropertyInfo).Returns(typeof(ITypedEntity).GetProperty(nameof(ITypedEntity.Type)));
            MappingsRepository.Setup(_ => _.FindPropertyMappingFor(It.IsAny<IEntity>(), It.IsAny<PropertyInfo>()))
                .Returns(propertyMapping.Object);
        }
    }
}
