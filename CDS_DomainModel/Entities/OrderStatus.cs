using System;
using System.Collections.Generic;

namespace CDS_DomainModel.Entities;

public partial class OrderStatus
{
    public int Id { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();
}
