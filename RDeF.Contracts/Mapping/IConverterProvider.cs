using System;

namespace RDeF.Mapping
{
    /// <summary>Describes an abstract converter providing facility.</summary>
    public interface IConverterProvider
    {
        /// <summary>Obtains a converter matching a given<paramref name="converterType" />.</summary>
        /// <param name="converterType">Type of the converter to obtain.</param>
        /// <returns>Instance of the converter best suitable for a given <paramref name="converterType" /> if matched; otherwise <b>null</b>.</returns>
        ILiteralConverter FindConverter(Type converterType);

        /// <summary>Obtains a literal converter for given <paramref name="valueType" />.</summary>
        /// <param name="valueType">Tpye of the value resulting converter should be capable of handling.</param>
        /// <returns>Instance of the literal converter capable of handling a given <paramref name="valueType" />.</returns>
        ILiteralConverter FindLiteralConverter(Type valueType);
    }
}
