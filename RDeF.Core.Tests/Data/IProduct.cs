using RDeF.Entities;
using RDeF.Mapping;
using RDeF.Mapping.Attributes;

namespace RDeF.Data
{
    [Class("Product")]
    public interface IProduct : IEntity
    {
        [Property("name", typeof(TestConverter))]
        string Name { get; set; }

        [Property("description")]
        string Description { get; set; }

        [Property("price")]
        double Price { get; set; }

        [Property("ordinal")]
        int Ordinal { get; set; }
    }
}
