using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using RDeF.Entities;
using RDeF.Vocabularies;
using RDeF.Xml;

namespace RDeF.Serialization
{
    /// <summary>Deserializies RDF/XML into a graph aligned statements.</summary>
    public class RdfXmlReader : IRdfReader
    {

        /// <inheritdoc />
        public async Task<IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>>> Read(StreamReader streamReader)
        {
            if (streamReader == null)
            {
                throw new ArgumentNullException(nameof(streamReader));
            }

            var statements = new List<Statement>();
            var blankNodes = new Dictionary<string, Iri>();
            using (var reader = XmlReader.Create(streamReader, new XmlReaderSettings() { Async = true }))
            {
                while (await reader.ReadAsync())
                {
                    if ((reader.NodeType == XmlNodeType.Element) && (reader.LocalName == "Description") && (reader.NamespaceURI == rdf.ns))
                    {
                        await ReadResource(reader, statements, blankNodes);
                    }
                }
            }

            return new[] { new KeyValuePair<Iri, IEnumerable<Statement>>(null, statements) };
        }

        private static async Task ReadResource(XmlReader reader, ICollection<Statement> statements, IDictionary<string, Iri> blankNodes)
        {
            Iri subject = reader.ReadSubject(blankNodes);
            while (await reader.ReadAsync())
            {
                switch (reader.NodeType)
                {
                   case XmlNodeType.Element:
                        await ReadPredicate(reader, subject, statements, blankNodes);
                        break;
                    case XmlNodeType.EndElement:
                        return;
                }
            }
        }

        private static async Task ReadPredicate(XmlReader reader, Iri subject, ICollection<Statement> statements, IDictionary<string, Iri> blankNodes)
        {
            bool isEmptyElement = reader.IsEmptyElement;
            Iri predicate = new Iri(reader.NamespaceURI + reader.LocalName);
            Iri @object = null;
            string value = null;
            string language = null;
            Iri datatype = null;
            ReadAttributes(reader, blankNodes, out @object, out value, out datatype, out language);
            if (isEmptyElement)
            {
                statements.Add(CreateStatement(subject, predicate, @object, value, datatype, language));
                return;
            }

            while (await reader.ReadAsync())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Text:
                        value = reader.Value;
                        break;
                    case XmlNodeType.EndElement:
                        statements.Add(CreateStatement(subject, predicate, @object, value, datatype, language));
                        return;
                }
            }
        }

        private static void ReadAttributes(XmlReader reader, IDictionary<string, Iri> blankNodes, out Iri @object, out string value, out Iri datatype, out string language)
        {
            @object = null;
            value = null;
            language = null;
            datatype = null;
            while (reader.MoveToNextAttribute())
            {
                if ((reader.Prefix == "xml") && (reader.LocalName == "lang"))
                {
                    language = reader.Value;
                    continue;
                }

                var iri = reader.ReadIri(blankNodes);
                if (iri == null)
                {
                    continue;
                }

                if (reader.LocalName == "datatype")
                {
                    datatype = iri;
                }
                else
                {
                    @object = iri;
                }
            }
        }

        private static Statement CreateStatement(Iri subject, Iri predicate, Iri @object, string value, Iri datatype, string language)
        {
            if (@object != null)
            {
                return new Statement(subject, predicate, @object);
            }

            if (language != null)
            {
                return new Statement(subject, predicate, value, language);
            }

            return new Statement(subject, predicate, value, datatype);
        }
    }
}
