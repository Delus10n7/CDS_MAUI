using System;
using System.Collections.Generic;

namespace CDS_DomainModel.Entities;

public partial class AvailabilityStatus
{
    public int Id { get; set; }

    public string AvailabilityName { get; set; } = null!;

    public virtual ICollection<Cars> Cars { get; set; } = new List<Cars>();
}
