using System;
using System.Collections.Generic;

namespace CDS_DomainModel.Entities;

public partial class Orders
{
    public int Id { get; set; }

    public bool? IsTradeIn { get; set; } = false;

    public decimal? TradeInValue { get; set; }

    public int? TradeInCarId { get; set; }

    public int ClientId { get; set; }

    public int ManagerId { get; set; }

    public int? CarId { get; set; }

    public DateOnly? OrderDate { get; set; }

    public int? StatusId { get; set; }

    public virtual Cars? Car { get; set; }

    public virtual UserBase Client { get; set; } = null!;

    public virtual UserBase Manager { get; set; } = null!;

    public virtual OrderStatus? Status { get; set; }

    public virtual Cars? TradeInCar { get; set; }
}
