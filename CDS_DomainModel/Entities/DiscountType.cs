using System;
using System.Collections.Generic;

namespace CDS_DomainModel.Entities;

public partial class DiscountType
{
    public int Id { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<Discount> Discount { get; set; } = new List<Discount>();
}
