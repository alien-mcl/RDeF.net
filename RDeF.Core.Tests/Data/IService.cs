using RDeF.Entities;
using RDeF.Mapping.Attributes;

namespace RDeF.Data
{
    [Class("Service")]
    public interface IService : IEntity
    {
        [Property("image")]
        string Image { get; set; }
    }
}
