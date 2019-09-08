using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using RDeF.Collections;
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

            var statements = new TypePrioritizingStatementCollection();
            using (var reader = XmlReader.Create(streamReader, new XmlReaderSettings() { Async = true }))
            {
                while (await reader.ReadAsync())
                {
                    if ((reader.NodeType == XmlNodeType.Element) && (reader.LocalName == "Description") && (reader.NamespaceURI == rdf.ns))
                    {
                        await ReadResource(reader, statements);
                    }
                }
            }

            return new[] { new KeyValuePair<Iri, IEnumerable<Statement>>(Iri.DefaultGraph, statements) };
        }

        private static async Task ReadResource(XmlReader reader, ICollection<Statement> statements)
        {
            Iri subject = reader.ReadSubject();
            while (await reader.ReadAsync())
            {
                switch (reader.NodeType)
                {
                   case XmlNodeType.Element:
                        await ReadPredicate(reader, subject, statements);
                        break;
                    case XmlNodeType.EndElement:
                        return;
                }
            }
        }

        private static async Task ReadPredicate(XmlReader reader, Iri subject, ICollection<Statement> statements)
        {
            bool isEmptyElement = reader.IsEmptyElement;
            Iri predicate = new Iri(reader.NamespaceURI + reader.LocalName);
            Iri @object;
            string value;
            string language;
            Iri datatype;
            ReadAttributes(reader, out @object, out value, out datatype, out language);
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

        private static void ReadAttributes(XmlReader reader, out Iri @object, out string value, out Iri datatype, out string language)
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

                var iri = reader.ReadIri();
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
