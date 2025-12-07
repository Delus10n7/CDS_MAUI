using System;
using System.Collections.Generic;

namespace CDS_DomainModel.Entities;

public partial class DriveType
{
    public int Id { get; set; }

    public string DriveName { get; set; } = null!;

    public virtual ICollection<CarConfiguration> CarConfiguration { get; set; } = new List<CarConfiguration>();
}
