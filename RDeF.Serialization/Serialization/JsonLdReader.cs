using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JsonLD.Core;
using JsonLD.Util;
using RDeF.Collections;
using RDeF.Entities;

namespace RDeF.Serialization
{
    /// <summary>Allows to read JSON-LD graphs.</summary>
    public class JsonLdReader : IRdfReader
    {
        private readonly JsonLdOptions _options;

        /// <summary>Initializes a new instance of the <see cref="JsonLdReader" /> class.</summary>
        public JsonLdReader() : this(null)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="JsonLdReader" /> class.</summary>
        /// <param name="options">Additional JSON-LD options.</param>
        public JsonLdReader(JsonLdOptions options)
        {
            _options = options ?? new JsonLdOptions(String.Empty);
        }

        /// <summary>Reads an RDF graph from a given <paramref name="streamReader" />.</summary>
        /// <param name="streamReader">Stream reader from which RDF data should be read.</param>
        /// <returns>Map of resources and their statements.</returns>
        [SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", Justification = "Searched string is culture invariant.")]
        public Task<IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>>> Read(StreamReader streamReader)
        {
            return Read(streamReader, CancellationToken.None);
        }

        /// <summary>Reads an RDF graph from a given <paramref name="streamReader" />.</summary>
        /// <param name="streamReader">Stream reader from which RDF data should be read.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Map of resources and their statements.</returns>
        [SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", Justification = "Searched string is culture invariant.")]
        public Task<IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>>> Read(StreamReader streamReader, CancellationToken cancellationToken)
        {
            if (streamReader == null)
            {
                throw new ArgumentNullException(nameof(streamReader));
            }

            var dataset = (RDFDataset)JsonLdProcessor.ToRDF(JSONUtils.FromReader(streamReader), _options);
            var defaultGraph = new TypePrioritizingStatementCollection();
            var graphMap = new Dictionary<Iri, IEnumerable<Statement>>();
            foreach (var graphName in dataset.GraphNames())
            {
                var graphIri = (graphName == "@default" ? Iri.DefaultGraph : new Iri(graphName));
                IEnumerable<Statement> statements;
                if (graphIri == Iri.DefaultGraph)
                {
                    statements = defaultGraph;
                }
                else if (!graphMap.TryGetValue(graphIri, out statements))
                {
                    graphMap[graphIri] = statements = new TypePrioritizingStatementCollection();
                }

                var statementsCollection = (ICollection<Statement>)statements;
                foreach (var quad in dataset.GetQuads(graphName))
                {
                    statementsCollection.Add(quad.AsStatement());
                }
            }

            return Task.FromResult(new[] { new KeyValuePair<Iri, IEnumerable<Statement>>(Iri.DefaultGraph, defaultGraph) }.Concat(graphMap));
        }
    }
}
