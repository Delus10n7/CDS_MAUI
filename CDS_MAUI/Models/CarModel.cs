using CDS_Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_MAUI.Models
{
    public class CarModel
    {
        public int Id { get; set; }
        public string? VIN { get; set; }
        public int? BrandId { get; set; }
        public string? Brand { get; set; }
        public int? ModelId { get; set; }
        public string? Model { get; set; }
        public int? Year { get; set; }
        public decimal? EngineVolume { get; set; }
        public int? Power { get; set; }
        public decimal? Price { get; set; }
        public string? Transmission { get; set; }
        public string? DriveType { get; set; }
        public string? EngineType { get; set; }
        public string? BodyType { get; set; }
        public string? Color { get; set; }
        public int? Mileage { get; set; }
        public string? Availability { get; set; }

        // Вычисляемые свойства для удобства привязки
        public string DisplayInfo => $"{Brand} {Model}";
        public string Specifications => $"{Year} год | {EngineVolume}л | {Power} л.с.";
        public string Features => $"{Transmission} | {DriveType}";
        public string FormattedPrice => $"{Price:N0} руб.";
        public string MileageInfo => $"{Mileage:N0} км";

        public CarModel(CarDTO c)
        {
            Id = c.Id;
            VIN = c.VIN;
            BrandId = c.BrandId;
            Brand = c.BrandName;
            ModelId = c.ModelId;
            Model = c.ModelName;
            Year = c.ReleaseYear;
            EngineVolume = c.EngineVolume;
            Power = c.EnginePower;
            Price = c.Price;
            Transmission = c.TransmissionName;
            DriveType = c.DriveTypeName;
            EngineType = c.EngineTypeName;
            BodyType = c.BodyTypeName;
            Color = c.CarColor;
            Mileage = c.Mileage;
            Availability = c.AvailabilityName;
        }
    }
}
