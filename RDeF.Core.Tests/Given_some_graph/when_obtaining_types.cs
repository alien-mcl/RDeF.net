﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
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
        public async Task Setup()
        {
            var context = new DefaultEntityContextFactory()
                .WithMappings(_ => _.FromAssemblyOf<ITypedEntity>())
                .Create();
            await ((ISerializableEntitySource)context.EntitySource).Read(new StreamReader(new MemoryStream()), new TestRdfReader());
            Entity = await context.Load<ITypedEntity>(Subject, CancellationToken.None);
        }

        private class TestRdfReader : IRdfReader
        {
            public Task<IEnumerable<IGraph>> Read(StreamReader streamReader, Uri baseUri = null)
            {
                return Read(streamReader, CancellationToken.None);
            }

            public Task<IEnumerable<IGraph>> Read(StreamReader streamReader, CancellationToken cancellationToken)
            {
                return Read(streamReader, null, CancellationToken.None);
            }

            public Task<IEnumerable<IGraph>> Read(StreamReader streamReader, Uri baseUri, CancellationToken cancellationToken)
            {
                return Task.FromResult<IEnumerable<IGraph>>(new List<IGraph>()
                {
                    new Graph(
                        Iri.DefaultGraph,
                        new[] { new Statement(Subject, rdf.type, TestClass), new Statement(Subject, rdf.type, AnotherTestClass) })
                });
            }
        }
    }
}
