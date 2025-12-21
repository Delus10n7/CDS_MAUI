using System;
using System.Collections.Generic;

namespace CDS_DomainModel.Entities;

public partial class ServiceContracts
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public int ManagerId { get; set; }

    public decimal? TotalPrice { get; set; }

    public DateOnly? SaleDate { get; set; }

    public string? ContractStatus { get; set; }

    public virtual UserBase Client { get; set; } = null!;

    public virtual UserBase Manager { get; set; } = null!;

    public virtual ICollection<SelectedService> SelectedService { get; set; } = new List<SelectedService>();
}
