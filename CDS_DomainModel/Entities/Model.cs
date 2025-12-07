using System;
using System.Collections.Generic;

namespace CDS_DomainModel.Entities;

public partial class Model
{
    public int Id { get; set; }

    public string? ModelName { get; set; }

    public int? BrandId { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual ICollection<CarConfiguration> CarConfiguration { get; set; } = new List<CarConfiguration>();

    public virtual ICollection<Discount> Discount { get; set; } = new List<Discount>();
}
