using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using RDeF.FluentAssertions;
using RDeF.Serialization;

namespace RDeF.Testing
{
    public abstract class RdfWriterTest<TWriter, TReader>
        where TWriter : IRdfWriter, new()
        where TReader : IRdfReader, new()
    {
        public IRdfWriter Writer { get; private set; }

        protected MemoryStream Stream { get; private set; }

        protected IEnumerable<IGraph> Graphs { get; set; }

        protected abstract bool UseStrictGraphMatching { get; }

        [Test]
        public void Should_serialize_to_Json_Ld_correctly_when_writing_named_graphs()
        {
            GivenSimpleGraph()
                .After(_ => _.Writer.Write(new StreamWriter(Stream) { AutoFlush = true }, Graphs))
                .WrittenGraph().Should().MatchStatements(Graphs, UseStrictGraphMatching);
        }

        [Test]
        public void Should_serialize_to_Json_Ld_correctly_when_writing_named_graphs_with_lists_and_collections()
        {
            GivenCollectionsGraph()
                .After(_ => _.Writer.Write(new StreamWriter(Stream) { AutoFlush = true }, Graphs))
                .WrittenGraph().Should().MatchStatements(Graphs, UseStrictGraphMatching);
        }

        [Test]
        public void Should_throw_when_null_is_given_instead_of_stream_writer()
        {
            Writer.Awaiting(instance => instance.Write(null, null))
                .ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Should_throw_when_null_is_given_instead_of_graphs()
        {
            Writer.Awaiting(instance => instance.Write(new StreamWriter(new MemoryStream()) { AutoFlush = true }, null))
                .ShouldThrow<ArgumentNullException>();
        }

        public IEnumerable<IGraph> WrittenGraph()
        {
            Stream.Flush();
            Stream.Seek(0, SeekOrigin.Begin);
            return new TReader().Read(new StreamReader(Stream)).Result;
        }

        [SetUp]
        public void Setup()
        {
            Writer = new TWriter();
            Stream = new MemoryStream();
        }

        protected RdfWriterTest<TWriter, TReader> GivenSimpleGraph()
        {
            Graphs = RdfTestSets.SimpleGraph;
            return this;
        }

        protected RdfWriterTest<TWriter, TReader> GivenCollectionsGraph()
        {
            Graphs = RdfTestSets.ComplexGraph;
            return this;
        }
    }
}
