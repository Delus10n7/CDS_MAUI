using System;
using System.Collections.Generic;

namespace CDS_DomainModel.Entities;

public partial class RoleType
{
    public int Id { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<Users> Users { get; set; } = new List<Users>();
}
