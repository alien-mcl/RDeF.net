using System.Collections.Generic;
using RDeF.Entities;

namespace RDeF.Data
{
    public interface IUnmappedProduct : IEntity
    {
        string Name { get; set; }

        string Description { get; set; }

        string Image { get; set; }

        ICollection<int> Ordinals { get; }

        ICollection<string> Categories { get; }

        ICollection<string> Comments { get; }
    }
}
