using System.Globalization;

namespace RDeF.Mapping.Converters
{
    internal static class ConverterExtensions
    {
        internal static T Using<T>(this T converter, string culture) where T : IConverter
        {
            CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = new CultureInfo(culture);
            return converter;
        }
    }
}
