using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Vocabularies;
using RollerCaster;

namespace Given_instance_of.DefaultChangeDetector_class.when_processing
{
    [TestFixture]
    internal class a_simple_entity : ScenarioTest
    {
        public override void TheTest()
        {
            Detector.Process(Entity, RetractedStatements, AddedStatements);
        }

        [Test]
        public void Should_not_detect_retracted_Description_statement()
        {
            RetractedStatements.Should().ContainKey(Entity).WhichValue.FirstOrDefault(statement => statement.Predicate.ToString() == "Description").Should().BeNull();
        }

        [Test]
        public void Should_detect_added_Description_statement()
        {
            AddedStatements.Should().ContainKey(Entity).WhichValue.First(statement => statement.Predicate.ToString() == "description").Value.Should().Be("New description");
        }

        [Test]
        public void Should_detect_retracted_Name_statement()
        {
            RetractedStatements.Should().ContainKey(Entity).WhichValue.First(statement => statement.Predicate.ToString() == "name").Value.Should().Be("Old name");
        }

        [Test]
        public void Should_not_detect_added_Name_statement()
        {
            AddedStatements.Should().ContainKey(Entity).WhichValue.FirstOrDefault(statement => statement.Predicate.ToString() == "Name").Should().BeNull();
        }

        [Test]
        public void Should_detect_added_Price_statement()
        {
            AddedStatements.Should().ContainKey(Entity).WhichValue.First(statement => statement.Predicate.ToString() == "price").Value.Should().Be("2.71828");
        }

        [Test]
        public void Should_detect_retracted_Price_statement()
        {
            RetractedStatements.Should().ContainKey(Entity).WhichValue.First(statement => statement.Predicate.ToString() == "price").Value.Should().Be("3.14159");
        }

        [Test]
        public void Should_detect_added_Ordinal_statement()
        {
            AddedStatements.Should().ContainKey(Entity).WhichValue.First(statement => statement.Predicate.ToString() == "ordinal").Value.Should().Be("7");
        }

        [Test]
        public void Should_detect_retracted_Ordinal_statement()
        {
            RetractedStatements.Should().ContainKey(Entity).WhichValue.First(statement => statement.Predicate.ToString() == "ordinal").Value.Should().Be("2");
        }

        [Test]
        public void Should_detect_retracted_type_statement()
        {
            RetractedStatements.Should().ContainKey(Entity).WhichValue.First(statement => statement.Predicate == rdfs.type).Object.ToString().Should().Be("Service");
        }

        [Test]
        public void Should_detect_added_type_statement()
        {
            AddedStatements.Should().ContainKey(Entity).WhichValue.First(statement => statement.Predicate == rdfs.type).Object.ToString().Should().Be("Thing");
        }

        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();
            Initialize(Entity);
        }

        private void Initialize(Entity entity)
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
