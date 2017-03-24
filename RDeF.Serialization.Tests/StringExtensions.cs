using System;
using System.Text.RegularExpressions;

namespace RDeF
{
    internal static class StringExtensions
    {
        internal static string Cleaned(this string json)
        {
            return Regex.Replace(json, "[ \r\n\t]", String.Empty);
        }
    }
}
