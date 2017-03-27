using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Entities;
using RDeF.Xml;

namespace Given_instance_of.RdfXmlWriter_class
{
    [TestFixture]
    public class when_writing_predicate
    {
        private XmlWriter Writer { get; set; }

        private IDictionary<string, string> Namespaces { get; set; }

        [Test]
        public void Should_throw_when_the_predicate_is_a_blank_node()
        {
            Writer.Awaiting(writer => writer.WriteStartElementAsync(new Iri(), Namespaces)).ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public async Task Should_generate_new_namespace_prefix_for_absolute_uri()
        {
            (await Awaiting(() => Writer.WriteStartElementAsync(new Iri("http://test.com/predicate"), Namespaces))).Should().ContainKey("ns1").WhichValue.Should().Be("http://test.com/");
        }

        [Test]
        public async Task Should_generate_new_namespace_prefix_for_absolute_uri_with_hash()
        {
            (await Awaiting(() => Writer.WriteStartElementAsync(new Iri("http://test.com/some#predicate"), Namespaces))).Should().ContainKey("ns1").WhichValue.Should().Be("http://test.com/some#");
        }

        [Test]
        public async Task Should_generate_new_namespace_prefix_for_absolute_uri_with_query_string()
        {
            (await Awaiting(() => Writer.WriteStartElementAsync(new Iri("http://test.com/some?predicate"), Namespaces))).Should().ContainKey("ns1").WhichValue.Should().Be("http://test.com/");
        }

        [Test]
        public async Task Should_not_generate_new_namespace_prefix_for_non_uri_predicate()
        {
            (await Awaiting(() => Writer.WriteStartElementAsync(new Iri("predicate"), Namespaces))).Should().NotContainKey("ns1");
        }

        [SetUp]
        public void Setup()
        {
            Namespaces = new Dictionary<string, string>() { { "ns0", "test" } };
            Writer = XmlWriter.Create(new MemoryStream(), new XmlWriterSettings() { Async = true });
        }

        private async Task<IDictionary<string, string>> Awaiting(Func<Task> action)
        {
            await action();
            return Namespaces;
        }
    }
}
