using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RDeF.Entities;
using RDeF.Vocabularies;

namespace RDeF.Serialization
{
    /// <summary>Writes RDF data as a JSON-LD.</summary>
    public class JsonLdWriter : IRdfWriter
    {
        private static readonly Iri[] UnquotedLiterals =
        {
            xsd.@byte,
            xsd.unsignedByte,
            xsd.@short,
            xsd.unsignedShort,
            xsd.@int,
            xsd.unsignedInt,
            xsd.@long,
            xsd.unsignedLong,
            xsd.@float,
            xsd.@double,
            xsd.@decimal,
            xsd.integer,
            xsd.positiveInteger,
            xsd.nonNegativeInteger,
            xsd.nonPositiveInteger,
            xsd.unsignedInteger,
            xsd.boolean
        };

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

            using (var jsonWriter = new JsonTextWriter(streamWriter))
            {
                await WriteInternal(jsonWriter, graphs);
            }
        }

        /// <summary>Serializes given <paramref name="graphs" /> into an <see cref="JsonWriter" />,</summary>
        /// <param name="jsonWriter">Target writer to receive data.</param>
        /// <param name="graphs">Graphs to be serialized.</param>
        /// <returns>Task of this operation.</returns>
        public async Task Write(JsonWriter jsonWriter, IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>> graphs)
        {
            if (jsonWriter == null)
            {
                throw new ArgumentNullException(nameof(jsonWriter));
            }

            if (graphs == null)
            {
                throw new ArgumentNullException(nameof(graphs));
            }

            await WriteInternal(jsonWriter, graphs);
        }

        private static async Task WriteGraph(JsonWriter jsonWriter, Iri graph, IEnumerable<Statement> statements)
        {
            await jsonWriter.WriteStartObjectAsync();
            if (graph != null)
            {
                await jsonWriter.WritePropertyNameAsync("@id");
                await jsonWriter.WriteValueAsync(graph != Iri.DefaultGraph ? (string)graph : "@default");
            }

            await jsonWriter.WritePropertyNameAsync("@graph");
            await jsonWriter.WriteStartArrayAsync();
            var subjects = statements.GroupBy(statement => statement.Subject).ToList();
            for (var index = 0; index < subjects.Count; index++)
            {
                var subject = subjects[index];
                await WriteSubject(jsonWriter, subject.Key, subject, subjects);
                if ((index < subjects.Count) && (subject != subjects[index]))
                {
                    index--;
                }
            }

            await jsonWriter.WriteEndArrayAsync();
            await jsonWriter.WriteEndObjectAsync();
        }

        private static async Task WriteSubject(JsonWriter jsonWriter, Iri subject, IEnumerable<Statement> statements, ICollection<IGrouping<Iri, Statement>> subjects)
        {
            await jsonWriter.WriteStartObjectAsync();
            if (subject != null)
            {
                await jsonWriter.WritePropertyNameAsync("@id");
                await jsonWriter.WriteValueAsync((string)subject);
            }

            foreach (var predicate in statements.GroupBy(statement => statement.Predicate).OrderBy(predicate => predicate.Key, IriComparer.Default))
            {
                await WritePredicate(jsonWriter, predicate.Key, predicate, subjects);
            }

            await jsonWriter.WriteEndObjectAsync();
        }

        private static async Task WritePredicate(JsonWriter jsonWriter, Iri predicate, IEnumerable<Statement> statements, ICollection<IGrouping<Iri, Statement>> subjects)
        {
            await jsonWriter.WritePropertyNameAsync(predicate == rdf.type ? "@type" : predicate);
            await jsonWriter.WriteStartArrayAsync();
            foreach (var value in statements)
            {
                if (value.IsLinkedList(subjects))
                {
                    await WriteList(jsonWriter, value, subjects);
                }
                else
                {
                    await WriteValue(jsonWriter, value);
                }
            }

            await jsonWriter.WriteEndArrayAsync();
        }

        private static async Task WriteList(JsonWriter jsonWriter, Statement list, ICollection<IGrouping<Iri, Statement>> subjects)
        {
            await jsonWriter.WriteStartObjectAsync();
            await jsonWriter.WritePropertyNameAsync("@list");
            await jsonWriter.WriteStartArrayAsync();
            if (list.Object != rdf.nil)
            {
                var item = subjects.FirstOrDefault(subject => subject.Key == list.Object);
                do
                {
                    subjects.Remove(item);
                    var value = item.FirstOrDefault(statement => statement.Predicate == rdf.first);
                    if (value != null)
                    {
                        await WriteValue(jsonWriter, value);
                    }

                    var rest = item.FirstOrDefault(statement => statement.Predicate == rdf.rest);
                    item = (rest != null ? subjects.FirstOrDefault(subject => subject.Key == rest.Object) : null);
                }
                while (item != null);
            }

            await jsonWriter.WriteEndArrayAsync();
            await jsonWriter.WriteEndObjectAsync();
        }

        private static async Task WriteValue(JsonWriter jsonWriter, Statement value)
        {
            if (value.Object != null)
            {
                if (value.Predicate == rdf.type)
                {
                    await jsonWriter.WriteValueAsync((string)value.Object);
                    return;
                }

                await jsonWriter.WriteStartObjectAsync();
                await jsonWriter.WritePropertyNameAsync("@id");
                await jsonWriter.WriteValueAsync((string)value.Object);
                await jsonWriter.WriteEndObjectAsync();
                return;
            }
            
            if (value.Language != null)
            {
                await jsonWriter.WriteStartObjectAsync();
                await jsonWriter.WritePropertyNameAsync("@language");
                await jsonWriter.WriteValueAsync(value.Language);
                await jsonWriter.WritePropertyNameAsync("@value");
                await WriteLiteral(jsonWriter, value);
                await jsonWriter.WriteEndObjectAsync();
                return;
            }

            if ((value.DataType != null) && (value.DataType != xsd.boolean) && (value.DataType != xsd.@integer) && (value.DataType != xsd.@double) && (value.DataType != xsd.@string))
            {
                await jsonWriter.WriteStartObjectAsync();
                await jsonWriter.WritePropertyNameAsync("@type");
                await jsonWriter.WriteValueAsync((string)value.DataType);
                await jsonWriter.WritePropertyNameAsync("@value");
                await WriteLiteral(jsonWriter, value);
                await jsonWriter.WriteEndObjectAsync();
                return;
            }

            await WriteLiteral(jsonWriter, value);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", Justification = "String used is culture invariant.")]
        private static async Task WriteLiteral(JsonWriter jsonWriter, Statement value)
        {
            if (UnquotedLiterals.Contains(value.DataType))
            {
                await jsonWriter.WriteRawValueAsync(value.Value + ((value.DataType == xsd.@double) && (value.Value.IndexOf(".") == -1) ? ".0" : String.Empty));
                return;
            }

            await jsonWriter.WriteValueAsync(value.Value);
        }

        private async Task WriteInternal(JsonWriter jsonWriter, IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>> graphs)
        {
            await jsonWriter.WriteStartObjectAsync();
            await jsonWriter.WritePropertyNameAsync("@graph");
            await jsonWriter.WriteStartArrayAsync();
            foreach (var graph in graphs)
            {
                await WriteGraph(jsonWriter, graph.Key, graph.Value);
            }

            await jsonWriter.WriteEndArrayAsync();
            await jsonWriter.WriteEndObjectAsync();
        }
    }
}
