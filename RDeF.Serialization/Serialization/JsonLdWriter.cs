using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
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
        public Task Write(StreamWriter streamWriter, IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>> graphs)
        {
            return Write(streamWriter, graphs, CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task Write(StreamWriter streamWriter, IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>> graphs, CancellationToken cancellationToken)
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
                await WriteInternal(jsonWriter, graphs, cancellationToken);
            }
        }

        /// <summary>Serializes given <paramref name="graphs" /> into an <see cref="JsonWriter" />,</summary>
        /// <param name="jsonWriter">Target writer to receive data.</param>
        /// <param name="graphs">Graphs to be serialized.</param>
        /// <returns>Task of this operation.</returns>
        public Task Write(JsonWriter jsonWriter, IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>> graphs)
        {
            return Write(jsonWriter, graphs, CancellationToken.None);
        }

        /// <summary>Serializes given <paramref name="graphs" /> into an <see cref="JsonWriter" />,</summary>
        /// <param name="jsonWriter">Target writer to receive data.</param>
        /// <param name="graphs">Graphs to be serialized.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task of this operation.</returns>
        public async Task Write(JsonWriter jsonWriter, IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>> graphs, CancellationToken cancellationToken)
        {
            if (jsonWriter == null)
            {
                throw new ArgumentNullException(nameof(jsonWriter));
            }

            if (graphs == null)
            {
                throw new ArgumentNullException(nameof(graphs));
            }

            await WriteInternal(jsonWriter, graphs, cancellationToken);
        }

        private static async Task WriteGraph(JsonWriter jsonWriter, Iri graph, IEnumerable<Statement> statements, CancellationToken cancellationToken)
        {
            await jsonWriter.WriteStartObjectAsync(cancellationToken);
            if (graph != null)
            {
                await jsonWriter.WritePropertyNameAsync("@id", cancellationToken);
                await jsonWriter.WriteValueAsync(graph != Iri.DefaultGraph ? (string)graph : "@default", cancellationToken);
            }

            await jsonWriter.WritePropertyNameAsync("@graph", cancellationToken);
            await jsonWriter.WriteStartArrayAsync(cancellationToken);
            var subjects = statements.GroupBy(statement => statement.Subject).ToList();
            for (var index = 0; index < subjects.Count; index++)
            {
                var subject = subjects[index];
                await WriteSubject(jsonWriter, subject.Key, subject, subjects, cancellationToken);
                if ((index < subjects.Count) && (subject != subjects[index]))
                {
                    index--;
                }
            }

            await jsonWriter.WriteEndArrayAsync(cancellationToken);
            await jsonWriter.WriteEndObjectAsync(cancellationToken);
        }

        private static async Task WriteSubject(
            JsonWriter jsonWriter,
            Iri subject,
            IEnumerable<Statement> statements,
            ICollection<IGrouping<Iri, Statement>> subjects,
            CancellationToken cancellationToken)
        {
            await jsonWriter.WriteStartObjectAsync(cancellationToken);
            if (subject != null)
            {
                await jsonWriter.WritePropertyNameAsync("@id", cancellationToken);
                await jsonWriter.WriteValueAsync((string)subject, cancellationToken);
            }

            foreach (var predicate in statements.GroupBy(statement => statement.Predicate).OrderBy(predicate => predicate.Key, IriComparer.Default))
            {
                await WritePredicate(jsonWriter, predicate.Key, predicate, subjects, cancellationToken);
            }

            await jsonWriter.WriteEndObjectAsync(cancellationToken);
        }

        private static async Task WritePredicate(
            JsonWriter jsonWriter,
            Iri predicate,
            IEnumerable<Statement> statements,
            ICollection<IGrouping<Iri, Statement>> subjects,
            CancellationToken cancellationToken)
        {
            await jsonWriter.WritePropertyNameAsync(predicate == rdf.type ? "@type" : predicate, cancellationToken);
            await jsonWriter.WriteStartArrayAsync(cancellationToken);
            foreach (var value in statements)
            {
                if (value.IsLinkedList(subjects))
                {
                    await WriteList(jsonWriter, value, subjects, cancellationToken);
                }
                else
                {
                    await WriteValue(jsonWriter, value, cancellationToken);
                }
            }

            await jsonWriter.WriteEndArrayAsync(cancellationToken);
        }

        private static async Task WriteList(
            JsonWriter jsonWriter,
            Statement list,
            ICollection<IGrouping<Iri, Statement>> subjects,
            CancellationToken cancellationToken)
        {
            await jsonWriter.WriteStartObjectAsync(cancellationToken);
            await jsonWriter.WritePropertyNameAsync("@list", cancellationToken);
            await jsonWriter.WriteStartArrayAsync(cancellationToken);
            if (list.Object != rdf.nil)
            {
                var item = subjects.FirstOrDefault(subject => subject.Key == list.Object);
                do
                {
                    subjects.Remove(item);
                    var value = item.FirstOrDefault(statement => statement.Predicate == rdf.first);
                    if (value != null)
                    {
                        await WriteValue(jsonWriter, value, cancellationToken);
                    }

                    var rest = item.FirstOrDefault(statement => statement.Predicate == rdf.rest);
                    item = (rest != null ? subjects.FirstOrDefault(subject => subject.Key == rest.Object) : null);
                }
                while (item != null);
            }

            await jsonWriter.WriteEndArrayAsync(cancellationToken);
            await jsonWriter.WriteEndObjectAsync(cancellationToken);
        }

        private static async Task WriteValue(JsonWriter jsonWriter, Statement value, CancellationToken cancellationToken)
        {
            if (value.Object != null)
            {
                if (value.Predicate == rdf.type)
                {
                    await jsonWriter.WriteValueAsync((string)value.Object, cancellationToken);
                    return;
                }

                await jsonWriter.WriteStartObjectAsync(cancellationToken);
                await jsonWriter.WritePropertyNameAsync("@id", cancellationToken);
                await jsonWriter.WriteValueAsync((string)value.Object, cancellationToken);
                await jsonWriter.WriteEndObjectAsync(cancellationToken);
                return;
            }
            
            if (value.Language != null)
            {
                await jsonWriter.WriteStartObjectAsync(cancellationToken);
                await jsonWriter.WritePropertyNameAsync("@language", cancellationToken);
                await jsonWriter.WriteValueAsync(value.Language, cancellationToken);
                await jsonWriter.WritePropertyNameAsync("@value", cancellationToken);
                await WriteLiteral(jsonWriter, value, cancellationToken);
                await jsonWriter.WriteEndObjectAsync(cancellationToken);
                return;
            }

            if ((value.DataType != null) && (value.DataType != xsd.boolean) && (value.DataType != xsd.@integer) && (value.DataType != xsd.@double) && (value.DataType != xsd.@string))
            {
                await jsonWriter.WriteStartObjectAsync(cancellationToken);
                await jsonWriter.WritePropertyNameAsync("@type", cancellationToken);
                await jsonWriter.WriteValueAsync((string)value.DataType, cancellationToken);
                await jsonWriter.WritePropertyNameAsync("@value", cancellationToken);
                await WriteLiteral(jsonWriter, value, cancellationToken);
                await jsonWriter.WriteEndObjectAsync(cancellationToken);
                return;
            }

            await WriteLiteral(jsonWriter, value, cancellationToken);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", Justification = "String used is culture invariant.")]
        private static async Task WriteLiteral(JsonWriter jsonWriter, Statement value, CancellationToken cancellationToken)
        {
            if (UnquotedLiterals.Contains(value.DataType))
            {
                await jsonWriter.WriteRawValueAsync(
                    value.Value + ((value.DataType == xsd.@double) && (value.Value.IndexOf(".") == -1) ? ".0" : String.Empty),
                    cancellationToken);
                return;
            }

            await jsonWriter.WriteValueAsync(value.Value, cancellationToken);
        }

        private async Task WriteInternal(
            JsonWriter jsonWriter,
            IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>> graphs,
            CancellationToken cancellationToken)
        {
            await jsonWriter.WriteStartObjectAsync(cancellationToken);
            await jsonWriter.WritePropertyNameAsync("@graph", cancellationToken);
            await jsonWriter.WriteStartArrayAsync(cancellationToken);
            foreach (var graph in graphs)
            {
                await WriteGraph(jsonWriter, graph.Key, graph.Value, cancellationToken);
            }

            await jsonWriter.WriteEndArrayAsync(cancellationToken);
            await jsonWriter.WriteEndObjectAsync(cancellationToken);
        }
    }
}
