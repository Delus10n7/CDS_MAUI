using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.DTO
{
    public class CarFilter
    {
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? BodyType { get; set; }
        public string? EngineType { get; set; }
        public string? Transmission { get; set; }
        public string? DriveType { get; set; }
        public string? Condition { get; set; }
        public string? Availability { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public decimal? MinEngineVolume { get; set; }
        public decimal? MaxEngineVolume { get; set; }
        public int? MinEnginePower { get; set; }
        public int? MaxEnginePower { get; set; }
        public int? MinYear { get; set; }
        public int? MaxYear { get; set; }
        public int? MinMileage { get; set; }
        public int? MaxMileage { get; set; }
        public string? SearchText { get; set; }
    }
}
