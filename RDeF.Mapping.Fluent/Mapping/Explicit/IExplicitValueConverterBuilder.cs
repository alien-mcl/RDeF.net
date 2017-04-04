using System.Diagnostics.CodeAnalysis;
using RDeF.Entities;

namespace RDeF.Mapping.Explicit
{
    /// <summary>Describes an abstract explicitely defined value conversion builder.</summary>
    /// <typeparam name="TEntity">Type of the entity being mapped.</typeparam>
    public interface IExplicitValueConverterBuilder<TEntity> where TEntity : IEntity
    {
        /// <summary>Allows to map current collection in scope to a value converter.</summary>
        /// <typeparam name="TConverter">Literal value converter type to be used.</typeparam>
        /// <returns>Current instance of the <see cref="IExplicitMappingsBuilder{TEntity}" /> that started this configuration.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "This is a part of a fluent-like API and works and intended.")]
        IExplicitMappingsBuilder<TEntity> WithValueConverter<TConverter>() where TConverter : ILiteralConverter;

        /// <summary>Allows to map current collection in scope to a default value converter.</summary>
        /// <returns>Current instance of the <see cref="IExplicitMappingsBuilder{TEntity}" /> that started this configuration.</returns>
        IExplicitMappingsBuilder<TEntity> WithDefaultConverter();
    }
}
