using System;
using System.Collections.Generic;

namespace CDS_DomainModel.Entities;

public partial class Country
{
    public int Id { get; set; }

    public string? CountryName { get; set; }

    public virtual ICollection<Brand> Brand { get; set; } = new List<Brand>();
}
