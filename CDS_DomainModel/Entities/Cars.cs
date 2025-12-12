using System;
using System.Collections.Generic;

namespace CDS_DomainModel.Entities;

public partial class Cars
{
    public int Id { get; set; }

    public string? VIN { get; set; }

    public int? Mileage { get; set; }

    public int? ReleaseYear { get; set; }

    public decimal? Price { get; set; }

    public string? CarColor { get; set; }

    public byte[]? Photo { get; set; }

    public int? ConditionId { get; set; }

    public int? AvailabilityId { get; set; }

    public int? ConfigurationId { get; set; }

    public virtual AvailabilityStatus? Availability { get; set; }

    public virtual ICollection<Booking> Booking { get; set; } = new List<Booking>();

    public virtual CarCondition? Condition { get; set; }

    public virtual CarConfiguration? Configuration { get; set; }

    public virtual ICollection<Orders> OrdersCar { get; set; } = new List<Orders>();

    public virtual ICollection<Orders> OrdersTradeInCar { get; set; } = new List<Orders>();
}
