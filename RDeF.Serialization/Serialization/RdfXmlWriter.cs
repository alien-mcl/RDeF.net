using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using RDeF.Entities;
using RDeF.Vocabularies;
using RDeF.Xml;

namespace RDeF.Serialization
{
    /// <summary>Provides a simple RDF over XML serialization.</summary>
    public class RdfXmlWriter : IRdfWriter
    {
        /// <inheritdoc />
        public async Task Write(StreamWriter streamWriter, IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>> graphs)
        {
            if (streamWriter == null)
            {
                throw new ArgumentNullException(nameof(streamWriter));
            }

            if (graphs == null)
            {
                throw new ArgumentNullException(nameof(graphs));
            }

            using (var xmlWriter = XmlWriter.Create(streamWriter, new XmlWriterSettings() { Async = true, IndentChars = "    ", Indent = true }))
            {
                await WriteInternal(xmlWriter, graphs);
            }
        }

        /// <summary>Serializes given <paramref name="graphs" /> into an <see cref="XmlWriter" />,</summary>
        /// <param name="xmlWriter">Target writer to receive data.</param>
        /// <param name="graphs">Graphs to be serialized.</param>
        /// <returns>Task of this operation.</returns>
        public async Task Write(XmlWriter xmlWriter, IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>> graphs)
        {
            if (xmlWriter == null)
            {
                throw new ArgumentNullException(nameof(xmlWriter));
            }

            if (graphs == null)
            {
                throw new ArgumentNullException(nameof(graphs));
            }

            await WriteInternal(xmlWriter, graphs);
        }

        private async Task WriteInternal(XmlWriter xmlWriter, IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>> graphs)
        {
            var namespaces = new Dictionary<string, string>();
            namespaces["rdf"] = rdf.ns;
            namespaces["rdfs"] = rdfs.ns;
            namespaces["xsd"] = xsd.ns;
            await xmlWriter.WriteProcessingInstructionAsync("xml", "version=\"1.0\" encoding=\"utf-8\"");
            await xmlWriter.WriteStartElementAsync("rdf", "RDF", namespaces["rdf"]);
            foreach (var graph in graphs)
            {
                await WriteGraph(xmlWriter, graph.Value, namespaces);
            }

            await xmlWriter.WriteEndElementAsync();
        }

        private async Task WriteGraph(XmlWriter xmlWriter, IEnumerable<Statement> statements, IDictionary<string, string> namespaces)
        {
            var subjects = statements.GroupBy(statement => statement.Subject).ToList();
            for (var index = 0; index < subjects.Count; index++)
            {
                var subject = subjects[index];
                await WriteSubject(xmlWriter, subject.Key, subject, subjects, namespaces);
                if ((index < subjects.Count) && (subject != subjects[index]))
                {
                    index--;
                }
            }
        }

        private async Task WriteSubject(
            XmlWriter xmlWriter,
            Iri subject,
            IEnumerable<Statement> statements,
            ICollection<IGrouping<Iri, Statement>> subjects,
            IDictionary<string, string> namespaces)
        {
            await xmlWriter.WriteStartElementAsync("rdf", "Description", namespaces["rdf"]);
            await xmlWriter.WriteAttributeStringAsync("rdf", (subject.IsBlank ? "nodeID" : "about"), namespaces["rdf"], subject);
            foreach (var predicate in statements.GroupBy(statement => statement.Predicate))
            {
                await WritePredicate(xmlWriter, predicate.Key, predicate, subjects, namespaces);
            }

            await xmlWriter.WriteEndElementAsync();
        }

        private async Task WritePredicate(
            XmlWriter xmlWriter,
            Iri predicate,
            IEnumerable<Statement> statements,
            ICollection<IGrouping<Iri, Statement>> subjects,
            IDictionary<string, string> namespaces)
        {
            foreach (var value in statements)
            {
                await xmlWriter.WriteStartElementAsync(predicate, namespaces);
                if (value.IsLinkedList(subjects))
                {
                    await xmlWriter.WriteAttributeStringAsync("rdf", "parseType", rdf.ns, "Collection");
                    await WriteList(xmlWriter, value, subjects, namespaces);
                }
                else
                {
                    await WriteValue(xmlWriter, value, namespaces);
                }

                await xmlWriter.WriteEndElementAsync();
            }
        }

        private async Task WriteList(XmlWriter xmlWriter, Statement list, ICollection<IGrouping<Iri, Statement>> subjects, IDictionary<string, string> namespaces)
        {
            if (list.Object == rdf.nil)
            {
                return;
            }

            var item = subjects.FirstOrDefault(subject => subject.Key == list.Object);
            do
            {
                subjects.Remove(item);
                var value = item.FirstOrDefault(statement => statement.Predicate == rdf.first);
                if (value != null)
                {
                    await WriteValue(xmlWriter, value, namespaces, true);
                }

                var rest = item.FirstOrDefault(statement => statement.Predicate == rdf.last);
                item = (rest != null ? subjects.FirstOrDefault(subject => subject.Key == rest.Object) : null);
            } while (item != null);
        }

        private async Task WriteValue(XmlWriter xmlWriter, Statement value, IDictionary<string, string> namespaces, bool requiresTag = false)
        {
            if (value.Object != null)
            {
                await xmlWriter.WriteAttributeStringAsync("rdf", (value.Object.IsBlank ? "nodeID" : "resource"), namespaces["rdf"], value.Object);
                return;
            }

            if (requiresTag)
            {
                await xmlWriter.WriteStartElementAsync("rdf", "li", namespaces["rdf"]);
            }

            await xmlWriter.WriteAttributeStringAsync("rdf", "parseType", namespaces["rdf"], "Literal");
            if (value.DataType != null)
            {
                await xmlWriter.WriteAttributeStringAsync("rdf", "datatype", namespaces["rdf"], value.DataType);
            }
            else if (value.Language != null)
            {
                await xmlWriter.WriteAttributeStringAsync("xml", "lang", String.Empty, value.Language);
            }

            await xmlWriter.WriteStringAsync(value.Value);
            if (requiresTag)
            {
                await xmlWriter.WriteEndElementAsync();
            }
        }
    }
}
