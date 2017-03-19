using RDeF.Entities;
using RDeF.Mapping.Attributes;

namespace RDeF.Data
{
    [Class(Iri = "Thing")]
    public interface IThing : IEntity
    {
        [Property(Iri = "abstract")]
        string Abstract { get; set; }
    }
}
