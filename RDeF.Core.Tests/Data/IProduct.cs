using System.Collections.Generic;
using RDeF.Entities;
using RDeF.Mapping.Attributes;
using RDeF.Mapping.Converters;

namespace RDeF.Data
{
    [Class(Iri = "some:Product")]
    public interface IProduct : IEntity
    {
        [Property(Iri = "some:name", ValueConverterType = typeof(TestConverter))]
        string Name { get; set; }

        [Property(Iri = "some:description")]
        string Description { get; set; }

        [Property(Iri = "some:price")]
        double Price { get; set; }

        [Property(Iri = "some:ordinal")]
        int Ordinal { get; set; }

        [Collection(Iri = "some:categories")]
        ICollection<string> Categories { get; }

        [Collection(Iri = "some:comments")]
        IList<string> Comments { get; }
    }
}
