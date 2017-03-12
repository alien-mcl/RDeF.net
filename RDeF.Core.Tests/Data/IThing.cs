using RDeF.Entities;
using RDeF.Mapping.Attributes;

namespace RDeF.Data
{
    [Class("Thing")]
    public interface IThing : IEntity
    {
        [Property("abstract")]
        string Abstract { get; set; }
    }
}
