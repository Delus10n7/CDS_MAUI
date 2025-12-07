using System;
using System.Collections.Generic;

namespace CDS_DomainModel.Entities;

public partial class BodyType
{
    public int Id { get; set; }

    public string BodyName { get; set; } = null!;

    public virtual ICollection<CarConfiguration> CarConfiguration { get; set; } = new List<CarConfiguration>();
}
