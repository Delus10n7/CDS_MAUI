using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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
        }
    }
}
