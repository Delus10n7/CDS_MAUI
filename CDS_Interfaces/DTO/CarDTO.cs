using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.DTO
{
    public class CarDTO
    {
        public int Id { get; set; }
        public string? VIN { get; set; }
        public int? Mileage { get; set; }
        public int? ReleaseYear { get; set; }
        public decimal? Price { get; set; }
        public string? CarColor { get; set; }
        public byte[]? Photo { get; set; }

        public int? ConditionId { get; set; }
        public int? AvailabilityId { get; set; }
        public int? ConfigurationId { get; set; }

        // Навигационные свойства
        public int? BrandId { get; set; }
        public string? BrandName { get; set; }
        public int? ModelId { get; set; }
        public string? ModelName { get; set; }
        public string? ConditionName { get; set; }
        public string? AvailabilityName { get; set; }
        public string? BodyTypeName { get; set; }
        public string? EngineTypeName { get; set; }
        public string? TransmissionName { get; set; }
        public string? DriveTypeName { get; set; }
        public decimal? EngineVolume { get; set; }
        public int? EnginePower { get; set; }

        public CarDTO() { }

        public CarDTO(Cars c)
        {
            Id = c.Id;
            VIN = c.VIN;
            Mileage = c.Mileage;
            ReleaseYear = c.ReleaseYear;
            Price = c.Price;
            CarColor = c.CarColor;
            Photo = c.Photo;
            ConditionId = c.ConditionId;
            AvailabilityId = c.AvailabilityId;
            ConfigurationId = c.ConfigurationId;

            // Навигационные свойства
            BrandId = c.Configuration.Model.BrandId;
            BrandName = c.Configuration.Model.Brand.BrandName;
            ModelId = c.Configuration.ModelId;
            ModelName = c.Configuration.Model.ModelName;
            ConditionName = c.Condition.ConditionName;
            AvailabilityName = c.Availability.AvailabilityName;
            BodyTypeName = c.Configuration.BodyType.BodyName;
            EngineTypeName = c.Configuration.EngineType.EngineName;
            TransmissionName = c.Configuration.TransmissionType.TransmissionName;
            DriveTypeName = c.Configuration.DriveType.DriveName;
            EngineVolume = c.Configuration.EngineVolume;
            EnginePower = c.Configuration.EnginePower;
        }
    }
}
