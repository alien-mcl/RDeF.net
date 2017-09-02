using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using RDeF.ComponentModel;

namespace RDeF.Entities
{
    /// <summary>Represents an International Resource Identifier.</summary>
    [TypeConverter(typeof(IriTypeConverter))]
    [DebuggerDisplay("{AsString,nq}")]
    public class Iri
    {
        private const string BlankSuffix = "_:blank";
        private static long _id;
        private readonly string _iri;
        private string _asString;

        /// <summary>Initializes a new instance of the <see cref="Iri" /> class.</summary>
        public Iri()
        {
            _iri = BlankSuffix + (++_id);
        }

        /// <summary>Initializes a new instance of the <see cref="Iri"/> class.</summary>
        /// <param name="uri">The URI.</param>
        public Iri(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            _iri = (Uri = uri).ToString();
        }

        /// <summary>Initializes a new instance of the <see cref="Iri"/> class.</summary>
        /// <param name="iri">The iri.</param>
        [SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads", Justification = "Type is a super class regarding System.Uri, this not all string representation are convertible to System.Uri.")]
        public Iri(string iri)
        {
            if (iri == null)
            {
                throw new ArgumentNullException(nameof(iri));
            }

            if (iri.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(iri));
            }

            _iri = iri;
        }

        /// <summary>Gets a value indicating whether this <see cref="Iri" /> is a blank identifier.</summary>
        [SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", MessageId = "System.String.StartsWith(System.String)", Justification = "Text being compared is culture invariant.")]
        public bool IsBlank { get { return _iri.StartsWith(BlankSuffix); } }

        internal Uri Uri { get; }

        [ExcludeFromCodeCoverage]
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "Debugging facility.")]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Debugging facility.")]
        internal string AsString
        {
            get { return _asString ?? (_asString = (IsBlank ? _iri : $"<{_iri}>")); }
        }

        /// <summary>Performs an implicit conversion from <see cref="Iri" /> to <see cref="Uri" />.</summary>
        /// <param name="iri">The International Resource Identifier to be converted.</param>
        /// <returns>The result of the conversion.</returns>n
        public static implicit operator Uri(Iri iri)
        {
            if (iri == null)
            {
                return null;
            }

            if (iri.Uri != null)
            {
                return iri.Uri;
            }

            Uri result;
            return (Uri.TryCreate(iri._iri, UriKind.RelativeOrAbsolute, out result) ? result : null);
        }

        /// <summary>Performs an implicit conversion from <see cref="Uri" /> to <see cref="Iri" />.</summary>
        /// <param name="uri">The Universal Resource Identifier to be converted.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Iri(Uri uri)
        {
            return (uri != null ? new Iri(uri) : null);
        }

        /// <summary>Performs an implicit conversion from <see cref="Iri"/> to <see cref="String" />.</summary>
        /// <param name="iri">The Universal Resource Identifier to be converted.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator string(Iri iri)
        {
            return iri?.ToString();
        }

        /// <summary>Compares two <see cref="Iri" />s for equality.</summary>
        /// <param name="operandA">Left operand.</param>
        /// <param name="operandB">Right operand.</param>
        /// <returns><b>true</b> if both operand are null or have equal International Resource Identifiers; otherwise <b>false</b>.</returns>
        public static bool operator ==(Iri operandA, Iri operandB)
        {
            return ((ReferenceEquals(operandA, null)) && (ReferenceEquals(operandB, null))) ||
                   (!(ReferenceEquals(operandA, null)) && !(ReferenceEquals(operandB, null)) &&
                   (operandA._iri == operandB._iri));
        }

        /// <summary>Compares two <see cref="Iri" />s for inequality.</summary>
        /// <param name="operandA">Left operand.</param>
        /// <param name="operandB">Right operand.</param>
        /// <returns><b>false</b> if both operand are null or have equal International Resource Identifiers; otherwise <b>true</b>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Argument is validated correctly.")]
        public static bool operator !=(Iri operandA, Iri operandB)
        {
            return ((ReferenceEquals(operandA, null)) && (!ReferenceEquals(operandB, null))) ||
                   ((!ReferenceEquals(operandA, null)) && (ReferenceEquals(operandB, null))) ||
                   (!(ReferenceEquals(operandA, null)) && (operandA._iri != operandB._iri));
        }

        /// <summary>Concatenates two <see cref="Iri" />s.</summary>
        /// <param name="operandA">Left operand.</param>
        /// <param name="operandB">Right operand.</param>
        /// <returns>Concatenated <see cref="Iri" />.</returns>
        public static Iri operator +(Iri operandA, Iri operandB)
        {
            if ((ReferenceEquals(operandA, null)) && (ReferenceEquals(operandB, null)))
            {
                return null;
            }

            if (ReferenceEquals(operandA, null))
            {
                return operandB;
            }

            if (ReferenceEquals(operandB, null))
            {
                return operandA;
            }

            if ((operandA.IsBlank) || (operandB.IsBlank))
            {
                throw new InvalidOperationException($"Unable to concatenate blank identifier.");
            }

            var uriA = operandA.Uri;
            var uriB = operandB.Uri;

            if ((uriA == null) || (uriB == null))
            {
                return new Iri(operandA._iri + operandB._iri);
            }

            if ((uriA.IsAbsoluteUri) && (uriB.IsAbsoluteUri))
            {
                throw new InvalidOperationException($"Unable to concatenate two absolute resource identifiers '{uriA}' and '{uriB}'.");
            }

            return new Iri(uriA.IsAbsoluteUri ? new Uri(uriA, uriB) : new Uri(uriB, uriA));
        }

        /// <summary>Concatenates an absolute international resource identifier with a relative one.</summary>
        /// <param name="operandA">The absolute international resource identifier.</param>
        /// <param name="operandB">The relative international resource identifier.</param>
        /// <returns>Concatenation of the international resource identifiers.</returns>
        public static Iri operator +(Iri operandA, string operandB)
        {
            if ((ReferenceEquals(operandA, null)) && (ReferenceEquals(operandB, null)))
            {
                return null;
            }

            if (operandB == null)
            {
                return operandA;
            }

            if (operandA == null)
            {
                throw new InvalidOperationException($"Unable to concatenate relative resource identifier '{operandB}' to missing absolute resource identifier.");
            }

            return (operandA.Uri != null ? new Iri(new Uri(operandA.Uri, new Uri(operandB, UriKind.Relative))) : new Iri(operandA._iri + operandB));
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            Iri anotherIri = obj as Iri;
            return (anotherIri != null && _iri.Equals(anotherIri._iri));
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return _iri.GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return _iri;
        }
    }
}