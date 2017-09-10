using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Entities;
using RDeF.Serialization;

namespace Given_instance_of.SimpleInMemoryEntitySource_class
{
    [TestFixture]
    public class when_deserializing : SimpleInMemoryEntitySourceTest
    {
        private static readonly Iri ExpectedSubject = new Iri("subject");

        private StreamReader StreamReader { get; set; }

        private Mock<IRdfReader> RdfReader { get; set; }

        private IEnumerable<Statement> ExpectedStatements { get; set; }

        public override void TheTest()
        {
            EntitySource.Read(StreamReader, RdfReader.Object).Wait();
        }

        [Test]
        public void Should_throw_when_no_input_stream_is_given()
        {
            EntitySource.Awaiting(instance => instance.Read(null, null))
                .ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("streamReader");
        }

        [Test]
        public void Should_throw_when_no_RDF_reader_is_given()
        {
            EntitySource.Awaiting(instance => instance.Read(new StreamReader(new MemoryStream()), null))
                .ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("rdfReader");
        }

        [Test]
        public void Should_use_RDF_reader()
        {
            RdfReader.Verify(instance => instance.Read(StreamReader), Times.Once);
        }

        [Test]
        public void Should_use_initialize_entity_with_statements()
        {
            Context.Verify(
                instance => instance.InitializeInternal(
                    It.Is<Entity>(entity => entity.Iri == ExpectedSubject),
                    ExpectedStatements,
                    It.IsAny<EntityInitializationContext>(),
                    It.IsAny<Action<Statement>>()),
                Times.Once);
        }

        [Test]
        public void Should_gather_all_entity_statements()
        {
            EntitySource.Entities.First().Value.First().Subject.Should().Be(ExpectedSubject);
        }

        [Test]
        public void Should_create_new_entity()
        {
            Context.Verify(
                instance => instance.CreateInternal(ExpectedSubject, true),
                Times.Once);
        }

        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();
            ExpectedStatements = new[] { new Statement(ExpectedSubject, new Iri("predicate"), String.Empty) };
            StreamReader = new StreamReader(new MemoryStream());
            RdfReader = new Mock<IRdfReader>(MockBehavior.Strict);
            RdfReader.Setup(instance => instance.Read(It.IsAny<StreamReader>()))
                .ReturnsAsync(new Dictionary<Iri, IEnumerable<Statement>>() { { new Iri("graph"), ExpectedStatements } });
            Context.Setup(instance => instance.Clear());
            Context.Setup(instance => instance.CreateInternal(It.IsAny<Iri>(), It.IsAny<bool>()))
                .Returns<Iri, bool>((iri, initialize) => (Entity)EntitySource.Create(iri));
            Context.Setup(instance => instance.InitializeInternal(
                    It.IsAny<Entity>(),
                    It.IsAny<IEnumerable<Statement>>(),
                    It.IsAny<EntityInitializationContext>(),
                    It.IsAny<Action<Statement>>()))
                .Callback<Entity, IEnumerable<Statement>, EntityInitializationContext, Action<Statement>>(
                    (entity, statements, context, handler) => statements.ToList().ForEach(handler));
        }
    }
}
