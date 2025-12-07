using System;
using System.Collections.Generic;

namespace CDS_DomainModel.Entities;

public partial class AdditionalService
{
    public int Id { get; set; }

    public string? ServiceName { get; set; }

    public decimal? Price { get; set; }

    public virtual ICollection<SelectedService> SelectedService { get; set; } = new List<SelectedService>();
}
