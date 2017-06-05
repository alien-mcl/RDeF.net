using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using RDeF.Entities;

namespace RDeF.Xml
{
    internal static class XmlWriterExtensions
    {
        internal static async Task WriteStartElementAsync(this XmlWriter xmlTextWriter, Iri iri, IDictionary<string, string> namespaces)
        {
            if (iri.IsBlank)
            {
                throw new ArgumentOutOfRangeException(nameof(iri));
            }

            var address = (string)iri;
            foreach (var @namespace in namespaces.Where(@namespace => address.StartsWith(@namespace.Value)))
            {
                await xmlTextWriter.WriteStartElementAsync(@namespace.Key, address.Substring(@namespace.Value.Length), @namespace.Value);
                return;
            }

            string newPrefix = "ns" + (namespaces.Keys.Where(prefix => prefix.StartsWith("ns")).Select(prefix => Int32.Parse(prefix.Substring(2))).FirstOrDefault() + 1);
            string uri = null;
            var position = address.IndexOf('#');
            if (position != -1)
            {
                uri = address.Substring(0, position + 1);
            }
            else if ((position = address.IndexOf('?')) != -1)
            {
                address = address.Substring(0, position);
            }

            if ((uri == null) && ((position = address.LastIndexOf('/')) != -1))
            {
                uri = address.Substring(0, position + 1);
            }

            if ((uri == null) && ((position = address.IndexOf(':')) != -1))
            {
                uri = address.Substring(0, position + 1);
            }
            
            if (uri == null)
            {
                await xmlTextWriter.WriteStartElementAsync(String.Empty, address, String.Empty);
                return;
            }

            await xmlTextWriter.WriteStartElementAsync(newPrefix, address.Substring(uri.Length), namespaces[newPrefix] = uri);
        }
    }
}
