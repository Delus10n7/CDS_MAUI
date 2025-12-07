using System;
using System.Collections.Generic;

namespace CDS_DomainModel.Entities;

public partial class Users
{
    public int Id { get; set; }

    public string UserLogin { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? FullName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public int RoleId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<Booking> Booking { get; set; } = new List<Booking>();

    public virtual ICollection<Discount> Discount { get; set; } = new List<Discount>();

    public virtual ICollection<Orders> OrdersClient { get; set; } = new List<Orders>();

    public virtual ICollection<Orders> OrdersManager { get; set; } = new List<Orders>();

    public virtual RoleType Role { get; set; } = null!;

    public virtual ICollection<ServiceContracts> ServiceContractsClient { get; set; } = new List<ServiceContracts>();

    public virtual ICollection<ServiceContracts> ServiceContractsManager { get; set; } = new List<ServiceContracts>();
}
