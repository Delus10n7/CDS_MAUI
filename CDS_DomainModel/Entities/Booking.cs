using System;
using System.Collections.Generic;

namespace CDS_DomainModel.Entities;

public partial class Booking
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int CarId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int BookingStatusId { get; set; }

    public virtual BookingStatus BookingStatus { get; set; } = null!;

    public virtual Cars Car { get; set; } = null!;

    public virtual UserBase User { get; set; } = null!;
}
