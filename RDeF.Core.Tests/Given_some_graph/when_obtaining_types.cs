using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Entities;
using RDeF.Serialization;
using RDeF.Vocabularies;

namespace Given_some_graph
{
    [TestFixture]
    public class when_obtaining_types
    {
        private static readonly Iri Subject = new Iri("http://temp.uri/subject");
        private static readonly Iri TestClass = new Iri("http://temp.uri/TestClass");
        private static readonly Iri AnotherTestClass = new Iri("http://temp.uri/AnotherTestClass");

        private ITypedEntity Entity { get; set; }

        [Test]
        public void Should_acknowledge_all_types()
        {
            Entity.Type.Should().BeEquivalentTo(TestClass, AnotherTestClass);
        }

        [SetUp]
        public void Setup()
        {
            var context = new DefaultEntityContextFactory()
                .WithMappings(_ => _.FromAssemblyOf<ITypedEntity>())
                .Create();
            ((ISerializableEntitySource)context.EntitySource).Read(new StreamReader(new MemoryStream()), new TestRdfReader());
            Entity = context.Load<ITypedEntity>(Subject);
        }

        private class TestRdfReader : IRdfReader
        {
            public Task<IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>>> Read(StreamReader streamReader)
            {
                return Task.FromResult((IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>>)new List<KeyValuePair<Iri, IEnumerable<Statement>>>()
                {
                    new KeyValuePair<Iri, IEnumerable<Statement>>(
                        Iri.DefaultGraph,
                        new[] { new Statement(Subject, rdf.type, TestClass), new Statement(Subject, rdf.type, AnotherTestClass) })
                });
            }
        }
    }
}
