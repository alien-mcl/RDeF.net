using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Mapping;
using RDeF.Vocabularies;
using RollerCaster;

namespace Given_instance_of.DefaultChangeDetector_class
{
    [TestFixture]
    internal class when_processing_an_entity : DefaultChangeDetectorTest
    {
        private IDictionary<IEntity, ISet<Statement>> _retractedStatements;
        private IDictionary<IEntity, ISet<Statement>> _addedStatements;

        private Entity Entity { get; set; }
        
        private Mock<IConverter> Converter { get; set; }

        public override void TheTest()
        {
            Detector.Process(Entity, ref _retractedStatements, ref _addedStatements);
        }

        [Test]
        public void Should_not_detect_retracted_Description_statement()
        {
            _retractedStatements.Should().ContainKey(Entity).WhichValue.FirstOrDefault(statement => statement.Predicate.ToString() == "Description").Should().BeNull();
        }

        [Test]
        public void Should_detect_added_Description_statement()
        {
            _addedStatements.Should().ContainKey(Entity).WhichValue.First(statement => statement.Predicate.ToString() == "Description").Value.Should().Be("New description");
        }

        [Test]
        public void Should_detect_retracted_Name_statement()
        {
            _retractedStatements.Should().ContainKey(Entity).WhichValue.First(statement => statement.Predicate.ToString() == "Name").Value.Should().Be("Old name");
        }

        [Test]
        public void Should_not_detect_added_Name_statement()
        {
            _addedStatements.Should().ContainKey(Entity).WhichValue.FirstOrDefault(statement => statement.Predicate.ToString() == "Name").Should().BeNull();
        }

        [Test]
        public void Should_detect_added_Price_statement()
        {
            _addedStatements.Should().ContainKey(Entity).WhichValue.First(statement => statement.Predicate.ToString() == "Price").Value.Should().Be("2.71828");
        }

        [Test]
        public void Should_detect_retracted_Price_statement()
        {
            _retractedStatements.Should().ContainKey(Entity).WhichValue.First(statement => statement.Predicate.ToString() == "Price").Value.Should().Be("3.14159");
        }

        [Test]
        public void Should_detect_added_Ordinal_statement()
        {
            _addedStatements.Should().ContainKey(Entity).WhichValue.First(statement => statement.Predicate.ToString() == "Ordinal").Value.Should().Be("7");
        }

        [Test]
        public void Should_detect_retracted_Ordinal_statement()
        {
            _retractedStatements.Should().ContainKey(Entity).WhichValue.First(statement => statement.Predicate.ToString() == "Ordinal").Value.Should().Be("2");
        }

        [Test]
        public void Should_detect_retracted_type_statement()
        {
            _retractedStatements.Should().ContainKey(Entity).WhichValue.First(statement => statement.Predicate == rdfs.type).Object.ToString().Should().Be("Service");
        }

        [Test]
        public void Should_detect_added_type_statement()
        {
            _addedStatements.Should().ContainKey(Entity).WhichValue.First(statement => statement.Predicate == rdfs.type).Object.ToString().Should().Be("Thing");
        }

        protected override void ScenarioSetup()
        {
            Converter = new Mock<IConverter>(MockBehavior.Strict);
            Converter.Setup(instance => instance.ConvertTo(It.IsAny<Iri>(), It.IsAny<Iri>(), It.IsAny<object>(), null))
                .Returns<Iri, Iri, object, Iri>((subject, predicate, value, graph) => new Statement(subject, predicate, String.Format(CultureInfo.InvariantCulture, "{0}", value), graph));
            MappingsRepository.Setup(instance => instance.FindEntityMappingFor(It.IsAny<Type>()))
                .Returns<Type>(type =>
                {
                    var entityMapping = new Mock<IEntityMapping>(MockBehavior.Strict);
                    entityMapping.SetupGet(instance => instance.Classes).Returns(new[] { new Iri(type.Name.Substring(1)) });
                    return entityMapping.Object;
                });
            MappingsRepository.Setup(instance => instance.FindPropertyMappingFor(It.IsAny<PropertyInfo>()))
                .Returns<PropertyInfo>(property =>
                {
                    var propertyMapping = new Mock<IPropertyMapping>(MockBehavior.Strict);
                    propertyMapping.SetupGet(instance => instance.ValueConverter).Returns(Converter.Object);
                    propertyMapping.SetupGet(instance => instance.Predicate).Returns(new Iri(property.Name));
                    return propertyMapping.Object;
                });
            _retractedStatements = new Dictionary<IEntity, ISet<Statement>>();
            _addedStatements = new Dictionary<IEntity, ISet<Statement>>();
            Entity = new Entity(new Iri("test"), new Mock<DefaultEntityContext>(null, MappingsRepository.Object, Detector).Object);
            InitializeEntity(Entity);
        }

        private void InitializeEntity(Entity entity)
        {
            var product = entity.ActLike<IProduct>();
            product.Name = "Old name";
            product.Price = 3.14159;
            product.Ordinal = 2;
            var service = entity.ActLike<IService>();
            service.Image = "Old image";
            Entity.IsInitialized = true;
            product.Name = null;
            product.Description = "New description";
            product.Price = 2.71828;
            product.Ordinal = 7;
            entity.UndoActLike<IService>();
            var thing = entity.ActLike<IThing>();
            thing.Abstract = "New abstract";
        }
    }
}
