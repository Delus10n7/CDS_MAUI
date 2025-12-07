using System;
using System.Collections.Generic;

namespace CDS_DomainModel.Entities;

public partial class Discount
{
    public int Id { get; set; }

    public int? ModelId { get; set; }

    public int? BrandId { get; set; }

    public int? ClientId { get; set; }

    public string? DiscountName { get; set; }

    public decimal? DiscountPercent { get; set; }

    public int? DiscountTypeId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual UserBase? Client { get; set; }

    public virtual DiscountType? DiscountType { get; set; }

    public virtual Model? Model { get; set; }
}
