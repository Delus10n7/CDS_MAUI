using System;
using System.Collections.Generic;

namespace CDS_DomainModel.Entities;

public partial class CarConfiguration
{
    public int Id { get; set; }

    public decimal? EngineVolume { get; set; }

    public int? EnginePower { get; set; }

    public int? ModelId { get; set; }

    public int? BodyTypeId { get; set; }

    public int? EngineTypeId { get; set; }

    public int? TransmissionTypeId { get; set; }

    public int? DriveTypeId { get; set; }

    public string? OtherDetails { get; set; }

    public virtual BodyType? BodyType { get; set; }

    public virtual ICollection<Cars> Cars { get; set; } = new List<Cars>();

    public virtual DriveType? DriveType { get; set; }

    public virtual EngineType? EngineType { get; set; }

    public virtual Model? Model { get; set; }

    public virtual TransmissionType? TransmissionType { get; set; }
}
