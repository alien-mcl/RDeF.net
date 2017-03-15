using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public void Write(StreamWriter streamWriter, IEnumerable<KeyValuePair<Iri, IEnumerable<Statement> >>graphs)
        {
            if (streamWriter == null)
            {
                throw new ArgumentNullException(nameof(streamWriter));
            }

            if (graphs == null)
            {
                throw new ArgumentNullException(nameof(graphs));
            }

            bool requiresGraphSeparator = false;
            WriteBegin(streamWriter);
            foreach (var graph in graphs)
            {
                WriteGraph(streamWriter, graph.Key, graph.Value, requiresGraphSeparator);
                requiresGraphSeparator = true;
            }

            WriteEnd(streamWriter);
            streamWriter.Flush();
        }

        private static void WriteBegin(StreamWriter streamWriter)
        {
            streamWriter.Write("{ ");
            streamWriter.Write("\"@graph\": [");
        }

        private static void WriteEnd(StreamWriter streamWriter)
        {
            streamWriter.Write("]}");
        }

        private static void WriteGraph(StreamWriter streamWriter, Iri graph, IEnumerable<Statement> statements, bool requiresGraphSeparator = true)
        {
            bool requiresSubjectSeparator = false;
            WriteBeginGraph(streamWriter, graph, requiresGraphSeparator);
            foreach (var subject in statements.GroupBy(statement => statement.Subject))
            {
                WriteSubject(streamWriter, subject.Key, subject, requiresSubjectSeparator);
                requiresSubjectSeparator = true;
            }
            
            WriteEndGraph(streamWriter);
        }

        private static void WriteBeginGraph(StreamWriter streamWriter, Iri graph, bool requiresSeparator)
        {
            if (requiresSeparator)
            {
                streamWriter.Write(",");
            }

            streamWriter.Write("{ ");
            if (graph != null)
            {
                streamWriter.Write("\"@id\": \"{0}\", ", graph);
            }

            streamWriter.Write("\"@graph\": [");
        }

        private static void WriteEndGraph(StreamWriter streamWriter)
        {
            streamWriter.Write("]}");
        }

        private static void WriteSubject(StreamWriter streamWriter, Iri subject, IEnumerable<Statement> statements, bool requiresSubjectSeparator = true)
        {
            bool requiresPredicateSeparator = false;
            WriteBeginSubject(streamWriter, subject, requiresSubjectSeparator);
            foreach (var predicate in statements.GroupBy(statement => statement.Predicate).OrderBy(predicate => predicate.Key, IriComparer.Default))
            {
                WritePredicate(streamWriter, predicate.Key, predicate, requiresPredicateSeparator);
                requiresPredicateSeparator = true;
            }

            WriteEndSubject(streamWriter);
        }

        private static void WriteBeginSubject(StreamWriter streamWriter, Iri subject, bool requiresSeparator)
        {
            if (requiresSeparator)
            {
                streamWriter.Write(",");
            }

            streamWriter.Write("{ ");
            if (subject != null)
            {
                streamWriter.Write("\"@id\": \"{0}\", ", subject);
            }
        }

        private static void WriteEndSubject(StreamWriter streamWriter)
        {
            streamWriter.Write(" }");
        }

        private static void WritePredicate(StreamWriter streamWriter, Iri predicate, IEnumerable<Statement> statements, bool requiresPredicateSeparator = true)
        {
            bool requiresValueSeparator = false;
            WriteBeginPredicate(streamWriter, predicate, requiresPredicateSeparator);
            foreach (var value in statements)
            {
                WriteValue(streamWriter, value, requiresValueSeparator);
                requiresValueSeparator = true;
            }

            WriteEndPredicate(streamWriter);
        }

        private static void WriteBeginPredicate(StreamWriter streamWriter, Iri predicate, bool requiresSeparator)
        {
            if (requiresSeparator)
            {
                streamWriter.Write(",");
            }

            if (predicate == rdfs.type)
            {
                streamWriter.Write("\"@type\": [");
                return;
            }

            streamWriter.Write("\"{0}\": [", predicate);
        }

        private static void WriteEndPredicate(StreamWriter streamWriter)
        {
            streamWriter.Write(" ]");
        }

        private static void WriteValue(StreamWriter streamWriter, Statement value, bool requiresValueSeparator = true)
        {
            if (requiresValueSeparator)
            {
                streamWriter.Write(", ");
            }

            if (value.Object != null)
            {
                if (value.Predicate == rdfs.type)
                {
                    streamWriter.Write("\"{0}\"", value.Object);
                    return;
                }

                streamWriter.Write("{{ \"@id\": \"{0}\" }}", value.Object);
                return;
            }

            if (value.DataType != null)
            {
                streamWriter.Write("{{ \"@type\": \"{0}\", \"@value\": {1} }}", value.DataType, FormatLiteral(value));
                return;
            }

            if (value.Language != null)
            {
                streamWriter.Write("{{ \"@lang\": \"{0}\", \"@value\": {1} }}", value.Language, FormatLiteral(value));
                return;
            }

            streamWriter.Write(FormatLiteral(value));
        }

        private static string FormatLiteral(Statement value)
        {
            return (UnquotedLiterals.Contains(value.DataType) ? value.Value : String.Format("\"{0}\"", value.Value));
        }
    }
}
