using System;
using System.Collections.Generic;

namespace CDS_DomainModel.Entities;

public partial class SelectedService
{
    public int Id { get; set; }

    public int ServiceContractId { get; set; }

    public int AdditionalServiceId { get; set; }

    public int? Quantity { get; set; }

    public decimal? TotalPrice { get; set; }

    public virtual AdditionalService AdditionalService { get; set; } = null!;

    public virtual ServiceContracts ServiceContract { get; set; } = null!;
}
