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

        // Навигационные свойства
        public string? BrandName { get; set; }
        public string? ModelName { get; set; }
        public string? BodyName { get; set; }
        public string? EngineName { get; set; }
        public string? TransmissionName { get; set; }
        public string? DriveName { get; set; }

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

            // Навигационные свойства
            BrandName = c.Model.Brand.BrandName;
            ModelName = c.Model.ModelName;
            BodyName = c.BodyType.BodyName;
            EngineName = c.EngineType.EngineName;
            TransmissionName = c.TransmissionType.TransmissionName;
            DriveName = c.DriveType.DriveName;
        }
    }
}
