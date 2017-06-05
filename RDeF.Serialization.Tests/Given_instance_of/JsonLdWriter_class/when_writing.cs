using System;
using System.IO;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using RDeF.Serialization;
using RDeF.Testing;

namespace Given_instance_of.JsonLdWriter_class
{
    [TestFixture]
    public class when_writing : RdfWriterTest<JsonLdWriter>
    {
        [Test]
        public void Should_throw_when_null_is_given_instead_of_stream_writer()
        {
            Writer.Awaiting(instance => instance.Write(null, null))
                .ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("streamWriter");
        }

        [Test]
        public void Should_throw_when_null_is_given_instead_of_graphs()
        {
            Writer.Awaiting(instance => instance.Write(new StreamWriter(new MemoryStream()), null))
                .ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("graphs");
        }

        [Test]
        public void Should_throw_when_null_is_given_instead_of_xml_writer()
        {
            ((JsonLdWriter)Writer).Awaiting(instance => instance.Write((JsonWriter)null, null))
                .ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("jsonWriter");
        }

        [Test]
        public void Should_throw_when_no_graphs_are_given()
        {
            ((JsonLdWriter)Writer).Awaiting(instance => instance.Write(new JsonTextWriter(new StreamWriter(new MemoryStream())), null))
                .ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("graphs");
        }
    }
}
