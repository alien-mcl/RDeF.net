using System;
using System.Linq.Expressions;
using RDeF.Entities;

namespace RDeF.Mapping.Explicit
{
    /// <summary>Describes an abstract explicitely defined mappings builder.</summary>
    /// <typeparam name="TEntity">Type of the entity being mapped.</typeparam>
    public interface IExplicitMappingsBuilder<TEntity> where TEntity : IEntity
    {
        /// <summary>Allows to map entity being created or loaded to explicitely defined map.</summary>
        /// <param name="term">Class to map entity to.</param>
        /// <param name="graph">Optional named graph to map to.</param>
        /// <returns>Current instance of the <see cref="IExplicitMappingsBuilder{TEntity}" /> that started this configuration.</returns>
        IExplicitMappingsBuilder<TEntity> MappedTo(Iri term, Iri graph = null);

        /// <summary>Allows to map entity being created or loaded to explicitely defined map.</summary>
        /// <param name="prefix">Prefix of the <paramref name="term"/>.</param>
        /// <param name="term">Term to map to.</param>
        /// <param name="graph">Optional named graph to map to.</param>
        /// <returns>Current instance of the <see cref="IExplicitMappingsBuilder{TEntity}" /> that started this configuration.</returns>
        IExplicitMappingsBuilder<TEntity> MappedTo(string prefix, string term, Iri graph = null);

        /// <summary>Allows to map entity being created or loaded to explicitely defined map.</summary>
        /// <param name="prefix">Prefix of the <paramref name="term"/>.</param>
        /// <param name="term">Term to map to.</param>
        /// <param name="graphPrefix">Prefix of the <paramref name="graphTerm"/>.</param>
        /// <param name="graphTerm">Graph term to map to.</param>
        /// <returns>Current instance of the <see cref="IExplicitMappingsBuilder{TEntity}" /> that started this configuration.</returns>
        IExplicitMappingsBuilder<TEntity> MappedTo(string prefix, string term, string graphPrefix, string graphTerm);

        /// <summary>Allows to map a collection of a given <paramref name="property" /> to some term.</summary>
        /// <typeparam name="TProperty">Type of the collection being mapped.</typeparam>
        /// <param name="property">Property to be mapped.</param>
        /// <returns>Instance of the <see cref="IExplicitPropertyMappingBuilder{TEntity}" /> allowing to map a selected <paramref name="property" /> to some term.</returns>
        IExplicitPropertyMappingBuilder<TEntity> WithProperty<TProperty>(Expression<Func<TEntity, TProperty>> property);

        /// <summary>Allows to map a collection of a given <paramref name="collection" /> to some term.</summary>
        /// <typeparam name="TProperty">Type of the collection being mapped.</typeparam>
        /// <param name="collection">Collection to be mapped.</param>
        /// <returns>Instance of the <see cref="IExplicitCollectionMappingBuilder{TEntity}" /> allowing to map a selected <paramref name="collection" /> to some term.</returns>
        IExplicitCollectionMappingBuilder<TEntity> WithCollection<TProperty>(Expression<Func<TEntity, TProperty>> collection);
    }
}
