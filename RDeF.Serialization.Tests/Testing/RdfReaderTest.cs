using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using RDeF.FluentAssertions;
using RDeF.Serialization;

namespace RDeF.Testing
{
    public abstract class RdfReaderTest<TReader, TWriter>
        where TReader : IRdfReader, new()
        where TWriter : IRdfWriter, new()
    {
        public IRdfReader Reader { get; private set; }

        public IEnumerable<IGraph> Result { get; private set; }

        protected Stream Stream { get; private set; }

        protected IEnumerable<IGraph> Expected { get; private set; }

        protected abstract bool UseStrictGraphMatching { get; }

        [Test]
        public void Should_deserialize_from_Json_Ld_correctly_when_deserializing_named_graphs()
        {
            GivenSimpleGraph()
                .After(_ => _.Result = _.Reader.Read(new StreamReader(Stream)).Result)
                .Result.Should().MatchStatements(Expected, UseStrictGraphMatching);
        }

        [Test]
        public void Should_deserialize_from_Json_Ld_correctly_when_deserializing_named_graphs_with_lists_and_collections()
        {
            GivenCollectionsGraph()
                .After(_ => _.Result = _.Reader.Read(new StreamReader(Stream)).Result)
                .Result.Should().MatchStatements(Expected, UseStrictGraphMatching);
        }

        [Test]
        public void Should_throw_when_null_is_given_instead_of_stream_reader()
        {
            Reader.Awaiting(instance => instance.Read(null))
                .ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("streamReader");
        }

        [SetUp]
        public void Setup()
        {
            Reader = new TReader();
        }

        protected RdfReaderTest<TReader, TWriter> GivenSimpleGraph()
        {
            return PrepareInputFor(RdfTestSets.SimpleGraph);
        }

        protected RdfReaderTest<TReader, TWriter> GivenCollectionsGraph()
        {
            return PrepareInputFor(RdfTestSets.ComplexGraph);
        }

        private RdfReaderTest<TReader, TWriter> PrepareInputFor(IEnumerable<IGraph> graphs)
        {
            new TWriter().Write(new StreamWriter(Stream = new MemoryStream()) { AutoFlush = true }, Expected = graphs).GetAwaiter().GetResult();
            Stream.Flush();
            Stream.Seek(0, SeekOrigin.Begin);
            return this;
        }
    }
}
