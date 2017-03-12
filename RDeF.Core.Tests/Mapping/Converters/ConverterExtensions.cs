﻿using System.Globalization;
using System.Threading;

namespace RDeF.Mapping.Converters
{
    internal static class ConverterExtensions
    {
        internal static T Using<T>(this T converter, string culture) where T : IConverter
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
            return converter;
        }
    }
}
