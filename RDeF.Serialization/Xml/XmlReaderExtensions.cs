using System;
using System.Collections.Generic;
using System.Xml;
using RDeF.Entities;
using RDeF.Vocabularies;

namespace RDeF.Xml
{
    internal static class XmlReaderExtensions
    {
        private const string NoSubjectMessage = "Unable to create a statement without a resource identifier.";

        internal static Iri ReadSubject(this XmlReader reader)
        {
            if (!reader.HasAttributes)
            {
                throw new InvalidOperationException(NoSubjectMessage);
            }

            while (reader.MoveToNextAttribute())
            {
                var iri = reader.ReadIri();
                if (iri != null)
                {
                    return iri;
                }
            }

            throw new InvalidOperationException(NoSubjectMessage);
        }

        internal static Iri ReadIri(this XmlReader reader)
        {
            if (reader.NamespaceURI != rdf.ns)
            {
                return null;
            }

            switch (reader.LocalName)
            {
                case "resource":
                case "datatype":
                case "about":
                    return new Iri(reader.Value);
                case "nodeID":
                    return new Iri($"_:{reader.Value}");
            }

            return null;
        }
    }
}
