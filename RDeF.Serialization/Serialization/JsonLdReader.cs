using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
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
        /// <inheritdoc />
        [SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", Justification = "Searched string is culture invariant.")]
        public Task<IEnumerable<KeyValuePair<Iri, IEnumerable<Statement>>>> Read(StreamReader streamReader)
        {
            if (streamReader == null)
            {
                throw new ArgumentNullException(nameof(streamReader));
            }

            var defaultGraph = new TypePrioritizingStatementCollection();
            var graphMap = new Dictionary<Iri, IEnumerable<Statement>>();
            var dataset = (RDFDataset)JsonLdProcessor.ToRDF(JSONUtils.FromReader(streamReader));
            foreach (var graphName in dataset.GraphNames())
            {
                var graphIri = (graphName == "@default" ? null : (graphName.StartsWith("_:") ? new Iri() : new Iri(graphName)));
                IEnumerable<Statement> statements;
                if (graphIri == null)
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

            return Task.FromResult(new[] { new KeyValuePair<Iri, IEnumerable<Statement>>(null, defaultGraph) }.Concat(graphMap));
        }
    }
}
