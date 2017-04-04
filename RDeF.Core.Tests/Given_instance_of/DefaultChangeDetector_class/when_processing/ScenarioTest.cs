using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Moq;
using RDeF.Entities;
using RDeF.Mapping;
using RollerCaster.Reflection;

namespace Given_instance_of.DefaultChangeDetector_class.when_processing
{
    internal abstract class ScenarioTest : DefaultChangeDetectorTest
    {
        protected IDictionary<IEntity, ISet<Statement>> RetractedStatements { get; private set; }

        protected IDictionary<IEntity, ISet<Statement>> AddedStatements { get; private set; }

        protected Mock<DefaultEntityContext> Context { get; private set; }

        protected Entity Entity { get; private set; }

        protected Mock<IConverter> Converter { get; private set; }

        protected override void ScenarioSetup()
        {
            Converter = new Mock<IConverter>(MockBehavior.Strict);
            Converter.Setup(instance => instance.ConvertTo(It.IsAny<Iri>(), It.IsAny<Iri>(), It.IsAny<object>(), null))
                .Returns<Iri, Iri, object, Iri>((subject, predicate, value, graph) => new Statement(subject, predicate, String.Format(CultureInfo.InvariantCulture, "{0}", value), graph));
            MappingsRepository.Setup(instance => instance.FindEntityMappingFor(It.IsAny<Type>()))
                .Returns<Type>(type =>
                {
                    var entityMapping = new Mock<IEntityMapping>(MockBehavior.Strict);
                    var classMapping = new Mock<IStatementMapping>(MockBehavior.Strict);
                    classMapping.SetupGet(instance => instance.Graph).Returns((Iri)null);
                    classMapping.SetupGet(instance => instance.Term).Returns(new Iri(type.Name.Substring(1)));
                    entityMapping.SetupGet(instance => instance.Classes).Returns(new[] { classMapping.Object });
                    return entityMapping.Object;
                });
            MappingsRepository.Setup(instance => instance.FindPropertyMappingFor(It.IsAny<PropertyInfo>()))
                .Returns<PropertyInfo>(property =>
                {
                    var propertyMapping = new Mock<IPropertyMapping>(MockBehavior.Strict);
                    propertyMapping.SetupGet(instance => instance.ValueConverter).Returns(Converter.Object);
                    propertyMapping.SetupGet(instance => instance.Term).Returns(new Iri(property.Name.ToLower()));
                    propertyMapping.SetupGet(instance => instance.Graph).Returns((Iri)null);
                    if (property.PropertyType.IsAnEnumerable())
                    {
                        var collectionMapping = propertyMapping.As<ICollectionMapping>();
                        collectionMapping.SetupGet(instance => instance.StoreAs)
                            .Returns(property.PropertyType.IsAList() ? CollectionStorageModel.LinkedList : CollectionStorageModel.Simple);
                        return collectionMapping.Object;
                    }

                    return propertyMapping.Object;
                });
            RetractedStatements = new Dictionary<IEntity, ISet<Statement>>();
            AddedStatements = new Dictionary<IEntity, ISet<Statement>>();
            Entity = new Entity(new Iri("test"), (Context = new Mock<DefaultEntityContext>(null, MappingsRepository.Object, Detector)).Object);
        }
    }
}
