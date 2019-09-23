using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Serialization;
using RDeF.Testing;

namespace Given_instance_of.RdfXmlWriter_class
{
    [TestFixture]
    public class when_initializing : RdfWriterTest<RdfXmlWriter>
    {
        [Test]
        public void Should_throw_when_null_is_given_instead_of_stream_writer()
        {
            Writer.Awaiting(instance => instance.Write(null, null))
                .ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Should_throw_when_null_is_given_instead_of_graphs()
        {
            Writer.Awaiting(instance => instance.Write(new StreamWriter(new MemoryStream()), null))
                .ShouldThrow<ArgumentNullException>();
        }
    }
}
