using System.Collections.Generic;
using RDeF.Entities;
using RDeF.Mapping;
using RDeF.Mapping.Attributes;

namespace RDeF.Data
{
    [Class(Iri = "Product")]
    public interface IProduct : IEntity
    {
        [Property(Iri = "name", ValueConverterType = typeof(TestConverter))]
        string Name { get; set; }

        [Property(Iri = "description")]
        string Description { get; set; }

        [Property(Iri = "price")]
        double Price { get; set; }

        [Property(Iri = "ordinal")]
        int Ordinal { get; set; }

        [Collection(Iri = "categories")]
        ICollection<string> Categories { get; }

        [Collection(Iri = "comments")]
        IList<string> Comments { get; }
    }
}
