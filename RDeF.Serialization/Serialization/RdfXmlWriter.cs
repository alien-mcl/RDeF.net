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
            int id = 0;
            namespaces["rdf"] = rdf.ns;
            namespaces["rdfs"] = rdfs.ns;
            namespaces["xsd"] = xsd.ns;
            await xmlWriter.WriteProcessingInstructionAsync("xml", "version=\"1.0\" encoding=\"utf-8\"");
            await xmlWriter.WriteStartElementAsync("rdf", "RDF", namespaces["rdf"]);
            foreach (var graph in graphs)
            {
                var context = new Context(graph.Value, namespaces, id);
                await WriteGraph(xmlWriter, context);
                id = context.Id;
            }

            await xmlWriter.WriteEndElementAsync();
        }

        private async Task WriteGraph(XmlWriter xmlWriter, Context context)
        {
            var subjects = context.Statements.GroupBy(statement => statement.Subject).ToList();
            for (var index = 0; index < subjects.Count; index++)
            {
                var subject = subjects[index];
                var childContext = new Context(subject, context.Namespaces, context.Id);
                await WriteSubject(xmlWriter, subject.Key, subjects, childContext);
                context.Id = childContext.Id;
                if ((index < subjects.Count) && (subject != subjects[index]))
                {
                    index--;
                }
            }
        }

        private async Task WriteSubject(XmlWriter xmlWriter, Iri subject, ICollection<IGrouping<Iri, Statement>> subjects, Context context)
        {
            await xmlWriter.WriteStartElementAsync("rdf", "Description", context.Namespaces["rdf"]);
            await xmlWriter.WriteAttributeStringAsync("rdf", (subject.IsBlank ? "nodeID" : "about"), context.Namespaces["rdf"], subject);
            foreach (var predicate in context.Statements.GroupBy(statement => statement.Predicate))
            {
                var childContext = new Context(predicate, context.Namespaces, context.Id);
                await WritePredicate(xmlWriter, predicate.Key, subjects, childContext);
                context.Id = childContext.Id;
            }

            await xmlWriter.WriteEndElementAsync();
        }

        private async Task WritePredicate(XmlWriter xmlWriter, Iri predicate, ICollection<IGrouping<Iri, Statement>> subjects, Context context)
        {
            var lists = new Dictionary<int, Statement>();
            Statement lastList = null;
            foreach (var value in context.Statements)
            {
                await xmlWriter.WriteStartElementAsync(predicate, context.Namespaces);
                if (value.IsLinkedList(subjects))
                {
                    int id = ++context.Id;
                    await xmlWriter.WriteAttributeStringAsync("rdf", "nodeID", context.Namespaces["rdf"], "bnode" + id);
                    lists[id] = lastList = value;
                }
                else
                {
                    await WriteValue(xmlWriter, value, context.Namespaces);
                }

                await xmlWriter.WriteEndElementAsync();
            }

            if (lists.Count > 0)
            {
                await xmlWriter.WriteEndElementAsync();
                foreach (var head in lists)
                {
                    await WriteList(xmlWriter, head.Value, "bnode" + head.Key, subjects, context);
                    if (head.Value != lastList)
                    {
                        await xmlWriter.WriteEndElementAsync();
                    }
                }
            }
        }

        private async Task WriteList(XmlWriter xmlWriter, Statement list, string id, ICollection<IGrouping<Iri, Statement>> subjects, Context context)
        {
            if (list.Object == rdf.nil)
            {
                return;
            }

            var item = subjects.FirstOrDefault(subject => subject.Key == list.Object);
            do
            {
                subjects.Remove(item);
                await xmlWriter.WriteStartElementAsync("rdf", "Description", context.Namespaces["rdf"]);
                await xmlWriter.WriteAttributeStringAsync("rdf", "nodeID", context.Namespaces["rdf"], id);
                var value = item.FirstOrDefault(statement => statement.Predicate == rdf.first);
                if (value != null)
                {
                    await xmlWriter.WriteStartElementAsync("rdf", "first", context.Namespaces["rdf"]);
                    await WriteValue(xmlWriter, value, context.Namespaces);
                    await xmlWriter.WriteEndElementAsync();
                }

                var rest = item.FirstOrDefault(statement => statement.Predicate == rdf.rest);
                await xmlWriter.WriteStartElementAsync("rdf", "rest", context.Namespaces["rdf"]);
                if (rest?.Object != rdf.nil)
                {
                    id = "bnode" + (++context.Id);
                    await xmlWriter.WriteAttributeStringAsync("rdf", "nodeID", context.Namespaces["rdf"], id);
                    item = subjects.FirstOrDefault(subject => subject.Key == rest.Object);
                    await xmlWriter.WriteEndElementAsync();
                }
                else
                {
                    await xmlWriter.WriteAttributeStringAsync("rdf", "resource", context.Namespaces["rdf"], rdf.nil);
                    await xmlWriter.WriteEndElementAsync();
                    return;
                }

                await xmlWriter.WriteEndElementAsync();
            }
            while (item != null);
        }

        private async Task WriteValue(XmlWriter xmlWriter, Statement value, IDictionary<string, string> namespaces)
        {
            if (value.Object != null)
            {
                await xmlWriter.WriteAttributeStringAsync("rdf", (value.Object.IsBlank ? "nodeID" : "resource"), namespaces["rdf"], value.Object);
                return;
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
        }

        private class Context
        {
            internal Context(IEnumerable<Statement> statements, IDictionary<string, string> namespaces, int id)
            {
                Id = id;
                Statements = statements;
                Namespaces = namespaces;
            }

            internal IEnumerable<Statement> Statements { get; }

            internal IDictionary<string, string> Namespaces { get; }

            internal int Id { get; set; }
        }
    }
}
