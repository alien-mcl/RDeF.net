using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;

namespace Given_instance_of.JsonLdWriter_class
{
    [TestFixture]
    public class when_initializing : JsonLdWriterTest
    {
        [Test]
        public void Should_throw_when_null_is_given_instead_of_stream_writer()
        {
            Writer.Invoking(instance => instance.Write(null, null)).ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("streamWriter");
        }

        [Test]
        public void Should_throw_when_null_is_given_instead_of_graphs()
        {
            Writer.Invoking(instance => instance.Write(new StreamWriter(new MemoryStream()), null)).ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("graphs");
        }
    }
}
