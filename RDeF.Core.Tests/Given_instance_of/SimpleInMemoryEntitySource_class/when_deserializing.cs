using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        private static readonly Iri ExpectedPredicate = new Iri("predicate");
        private static readonly Iri ExpectedProperty = new Iri("assert");
        private static readonly Iri ExpectedObject = new Iri("object");

        private static readonly Statement ExpectedStatement = new Statement(ExpectedSubject, ExpectedPredicate, ExpectedObject);

        private static readonly Statement ExpectedAdditionalStatement = new Statement(ExpectedSubject, ExpectedProperty, "Test");

        private StreamReader StreamReader { get; set; }

        private Mock<IRdfReader> RdfReader { get; set; }

        private IEnumerable<Statement> ExpectedStatements { get; set; }

        public override async Task TheTest()
        {
            await EntitySource.Read(StreamReader, RdfReader.Object);
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
            RdfReader.Verify(instance => instance.Read(StreamReader, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void Should_use_initialize_entity_with_statements()
        {
            Context.Verify(
                instance => instance.InitializeInternal(
                    It.Is<Entity>(entity => entity.Iri == ExpectedSubject),
                    ExpectedStatements,
                    It.IsAny<EntityInitializationContext>(),
                    It.IsAny<Action<Statement>>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public void Should_use_initialize_predicate()
        {
            Context.Verify(
                instance => instance.InitializeInternal(
                    It.Is<Entity>(entity => entity.Iri == ExpectedPredicate),
                    It.Is<IEnumerable<Statement>>(set => !set.Any()),
                    It.IsAny<EntityInitializationContext>(),
                    It.IsAny<Action<Statement>>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public void Should_use_initialize_property()
        {
            Context.Verify(
                instance => instance.InitializeInternal(
                    It.Is<Entity>(entity => entity.Iri == ExpectedProperty),
                    It.Is<IEnumerable<Statement>>(set => !set.Any()),
                    It.IsAny<EntityInitializationContext>(),
                    It.IsAny<Action<Statement>>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public void Should_use_initialize_entity_without_statements()
        {
            Context.Verify(
                instance => instance.InitializeInternal(
                    It.Is<Entity>(entity => entity.Iri == ExpectedObject),
                    It.Is<IEnumerable<Statement>>(set => !set.Any()),
                    It.IsAny<EntityInitializationContext>(),
                    It.IsAny<Action<Statement>>(),
                    It.IsAny<CancellationToken>()),
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
                instance => instance.CreateInternal(ExpectedSubject, true, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();
            ExpectedStatements = new[] { ExpectedStatement, ExpectedAdditionalStatement };
            StreamReader = new StreamReader(new MemoryStream());
            RdfReader = new Mock<IRdfReader>(MockBehavior.Strict);
            RdfReader.Setup(instance => instance.Read(It.IsAny<StreamReader>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Dictionary<Iri, IEnumerable<Statement>>() { { new Iri("graph"), new[] { ExpectedStatement } } });
            Context.Setup(instance => instance.Clear());
            Context.Setup(instance => instance.CreateInternal(It.IsAny<Iri>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .Returns<Iri, bool, CancellationToken>(
                    (iri, initialize, token) => Task.FromResult((Entity)EntitySource.Create(iri, token).Result));
            Context.Setup(instance => instance.InitializeInternal(
                    It.IsAny<Entity>(),
                    It.IsAny<IEnumerable<Statement>>(),
                    It.IsAny<EntityInitializationContext>(),
                    It.IsAny<Action<Statement>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Callback<Entity, IEnumerable<Statement>, EntityInitializationContext, Action<Statement>, CancellationToken>(
                    (entity, statements, context, handler, token) => statements.ToList().ForEach(handler));
            EntitySource.StatementAsserted += (sender, e) => e.AdditionalStatementsToAssert.Add(ExpectedAdditionalStatement);
        }
    }
}
