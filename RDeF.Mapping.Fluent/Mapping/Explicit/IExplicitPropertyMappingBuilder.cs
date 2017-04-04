using RDeF.Entities;

namespace RDeF.Mapping.Explicit
{
    /// <summary>Describes an abstract explicitely defined property mapping builder.</summary>
    /// <typeparam name="TEntity">Type of the entity being mapped.</typeparam>
    public interface IExplicitPropertyMappingBuilder<TEntity> where TEntity : IEntity
    {
        /// <summary>Allows to map current property in scope to a given <paramref name="term" />.</summary>
        /// <param name="term">Term to map to.</param>
        /// <param name="graph">Named graph to map to.</param>
        /// <returns>Instance of the <see cref="IExplicitValueConverterBuilder{TEntity}" /> that will allow to configure a value converter.</returns>
        IExplicitValueConverterBuilder<TEntity> MappedTo(Iri term, Iri graph = null);
    }
}
