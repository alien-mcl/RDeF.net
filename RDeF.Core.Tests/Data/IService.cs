using RDeF.Entities;
using RDeF.Mapping.Attributes;

namespace RDeF.Data
{
    [Class(Iri = "Service")]
    public interface IService : IEntity
    {
        [Property(Iri = "image")]
        string Image { get; set; }
    }
}
