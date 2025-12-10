using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.DTO
{
    public class CarConfigurationDTO
    {
        public int Id { get; set; }
        public decimal? EngineVolume { get; set; }
        public int? EnginePower { get; set; }
        public string? OtherDetails { get; set; }

        public int? ModelId { get; set; }
        public int? BodyTypeId { get; set; }
        public int? EngineTypeId { get; set; }
        public int? TransmissionTypeId { get; set; }
        public int? DriveTypeId { get; set; }

        public CarConfigurationDTO() { }

        public CarConfigurationDTO(CarConfiguration c)
        {
            Id = c.Id;
            EngineVolume = c.EngineVolume;
            EnginePower = c.EnginePower;
            OtherDetails = c.OtherDetails;

            ModelId = c.ModelId;
            BodyTypeId = c.BodyTypeId;
            EngineTypeId = c.EngineTypeId;
            TransmissionTypeId = c.TransmissionTypeId;
            DriveTypeId = c.DriveTypeId;
        }
    }
}
