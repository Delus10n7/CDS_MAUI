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

        public ModelDTO? Model { get; set; }
        public BodyTypeDTO? BodyType { get; set; }
        public EngineTypeDTO? EngineType { get; set; }
        public TransmissionTypeDTO? TransmissionType { get; set; }
        public DriveTypeDTO? DriveType { get; set; }

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

            Model = c.Model != null ? new ModelDTO(c.Model) : null;
            BodyType = c.BodyType != null ? new BodyTypeDTO(c.BodyType) : null;
            EngineType = c.EngineType != null ? new EngineTypeDTO(c.EngineType) : null;
            TransmissionType = c.TransmissionType != null ? new TransmissionTypeDTO(c.TransmissionType) : null;
            DriveType = c.DriveType != null ? new DriveTypeDTO(c.DriveType) : null;
        }
    }
}
