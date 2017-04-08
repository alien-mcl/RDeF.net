#pragma warning disable SA1300 // Element must begin with upper-case letter
#pragma warning disable SA1307 // Accessible fields must begin with upper-case letter
#pragma warning disable SA1311 // Static readonly fields must begin with upper-case letter
#pragma warning disable SA1303 // Const field names must begin with upper-case letter
using System.Diagnostics.CodeAnalysis;
using RDeF.Entities;

namespace RDeF.Vocabularies
{
    /// <summary>Exposes Open GUID terms.</summary>
    [ExcludeFromCodeCoverage]
    [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "No testable logic.")]
    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with Open GUID namespace convention.")]
    public static class oguid
    {
        /// <summary>Defines Open GUID base url.</summary>
        public const string ns = "http://openguid.net/rdf#";

        /// <summary>Defines Open GUID base url.</summary>
        public static readonly Iri Namespace = new Iri(ns);
    }
}
#pragma warning restore SA1303 // Const field names must begin with upper-case letter
#pragma warning restore SA1311 // Static readonly fields must begin with upper-case letter
#pragma warning restore SA1307 // Accessible fields must begin with upper-case letter
#pragma warning restore SA1300 // Element must begin with upper-case letter
