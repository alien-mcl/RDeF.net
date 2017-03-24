using System.Collections.Generic;
using System.IO;
using System.Linq;
using Moq;
using NUnit.Framework;
using RDeF.Entities;
using RDeF.Mapping;
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

        public override void TheTest()
        {
            EntitySource.Write(Buffer, RdfWriter.Object);
        }

        [Test]
        public void Should_serialize_statements_grouped_by_graph()
        {
            RdfWriter.Verify(instance => instance.Write(It.IsAny<StreamWriter>(), It.Is<IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>>>(collection => Test(collection))));
        }

        [Test]
        public void Should_serialize_into_a_given_stream()
        {
            RdfWriter.Verify(instance => instance.Write(Buffer, It.IsAny<IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>>>()));
        }

        protected override void ScenarioSetup()
        {
            Buffer = new StreamWriter(new MemoryStream());
            RdfWriter = new Mock<IRdfWriter>(MockBehavior.Strict);
            RdfWriter.Setup(instance => instance.Write(It.IsAny<StreamWriter>(), It.IsAny<IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>>>()));
            var entityContext = new DefaultEntityContext(EntitySource, new Mock<IMappingsRepository>(MockBehavior.Strict).Object, new Mock<IChangeDetector>(MockBehavior.Strict).Object);
            var entity = new Entity(new Iri("test"), entityContext);
            EntitySource.Entities[entity] = new HashSet<Statement>()
            {
                new Statement(entity.Iri, new Iri("predicate"), entity.Iri, PrimaryGraph),
                new Statement(entity.Iri, new Iri("predicate"), entity.Iri, SecondaryGraph)
            };
        }

        private bool Test(IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>> collection)
        {
            return (from graph in collection
                    join graphIri in new[] { PrimaryGraph, SecondaryGraph } on graph.Key equals graphIri into matched
                    select matched).Count() == 2;
        }
    }
}
