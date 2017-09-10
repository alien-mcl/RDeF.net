using System;
using RDeF.Mapping.Entities;
using RDeF.Mapping.Explicit;

namespace RDeF.Entities
{
    /// <summary>Exposes useful <see cref="UnmappedPropertyEventArgs" /> extension methods.</summary>
    public static class UnmappedPropertyEventArgsExtensions
    {
        /// <summary>Instructs to map a given predicate to a given <typeparamref name="T" />.</summary>
        /// <typeparam name="T">Type of the entity owning the property to be mapped.</typeparam>
        /// <param name="e">Event arguments.</param>
        /// <param name="mappingsBuilder">Mappings builder.</param>
        public static void OfEntity<T>(this UnmappedPropertyEventArgs e, Action<IExplicitMappingsBuilder<T>> mappingsBuilder) where T : IEntity
        {
            if (e != null)
            {
                e.PropertyMapping = e.EntityContext.BuildExplicitMappings(mappingsBuilder, e.Statement.Subject, true);
            }
        }
    }
}
