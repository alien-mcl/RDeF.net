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
    public class when_serializing : SimpleInMemoryEntitySourceTest
    {
        private static readonly Iri PrimaryGraph = new Iri("graph1");
        private static readonly Iri SecondaryGraph = new Iri("graph2");

        private StreamWriter Buffer { get; set; }

        private Mock<IRdfWriter> RdfWriter { get; set; }

        private ISet<Statement> ExpectedStatements { get; set; }

        public override async Task TheTest()
        {
            await EntitySource.Write(Buffer, RdfWriter.Object);
        }

        [Test]
        public void Should_serialize_statements_grouped_by_graph()
        {
            RdfWriter.Verify(
                instance => instance.Write(
                    It.IsAny<StreamWriter>(),
                    It.Is<IEnumerable<IGraph>>(collection => Match(collection)),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public void Should_serialize_into_a_given_stream()
        {
            RdfWriter.Verify(
                instance => instance.Write(Buffer, It.IsAny<IEnumerable<IGraph>>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public void Should_throw_when_no_StreamWriter_is_given()
        {
            EntitySource.Awaiting(instance => instance.Write(null, null))
                .Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("streamWriter");
        }

        [Test]
        public void Should_throw_when_no_RdfWriter_is_given()
        {
            EntitySource.Awaiting(instance => instance.Write(new StreamWriter(new MemoryStream()), null))
                .Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("rdfWriter");
        }

        [Test]
        public void Should_retrieve_all_statements()
        {
            EntitySource.Statements.Should().BeEquivalentTo(ExpectedStatements);
        }

        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();
            Buffer = new StreamWriter(new MemoryStream());
            RdfWriter = new Mock<IRdfWriter>(MockBehavior.Strict);
            RdfWriter
                .Setup(instance => instance.Write(
                    It.IsAny<StreamWriter>(),
                    It.IsAny<IEnumerable<IGraph>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            var entity = new Entity(new Iri("test"), Context.Object);
            EntitySource.Entities[EntitySource.EntityMap[entity.Iri] = entity] = ExpectedStatements = new HashSet<Statement>()
            {
                new Statement(entity.Iri, new Iri("predicate"), entity.Iri, PrimaryGraph),
                new Statement(entity.Iri, new Iri("predicate"), entity.Iri, SecondaryGraph)
            };
        }

        private bool Match(IEnumerable<IGraph> collection)
        {
            return (from graph in collection
                    join graphIri in new[] { PrimaryGraph, SecondaryGraph } on graph.Iri equals graphIri into matched
                    select matched).Count() == 2;
        }
    }
}
