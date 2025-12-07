using System;
using System.Collections.Generic;

namespace CDS_DomainModel.Entities;

public partial class BookingStatus
{
    public int Id { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<Booking> Booking { get; set; } = new List<Booking>();
}
