using System.Collections.Generic;
using RDeF.Entities;

namespace RDeF.Data
{
    public interface IUnmappedProduct : IEntity
    {
        string Name { get; set; }

        string Description { get; set; }

        ICollection<string> Categories { get; }
    }
}
