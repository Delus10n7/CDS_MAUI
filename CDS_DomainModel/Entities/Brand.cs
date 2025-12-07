using System;
using System.Collections.Generic;

namespace CDS_DomainModel.Entities;

public partial class Brand
{
    public int Id { get; set; }

    public string? BrandName { get; set; }

    public int? CountryId { get; set; }

    public virtual Country? Country { get; set; }

    public virtual ICollection<Discount> Discount { get; set; } = new List<Discount>();

    public virtual ICollection<Model> Model { get; set; } = new List<Model>();
}
