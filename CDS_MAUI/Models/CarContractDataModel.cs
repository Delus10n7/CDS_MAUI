using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_MAUI.Models
{
    public class CarContractDataModel
    {
        public string CarDealershipCity { get; set; } = "Иваново";
        public DateTime ContractDate { get; set; } = DateTime.Now;

        // Покупатель
        public string CustomerFullName { get; set; }

        // Автомобиль
        public string CarBrand { get; set; }
        public string CarModel { get; set; }
        public string CarVIN { get; set; }
        public string CarReleaseYear { get; set; }
        public string CarMileage { get; set; }
        public string CarEngineVolume { get; set; }
        public string CarEnginePower { get; set; }
        public string CarEngineType { get; set; }
        public string CarTransmissionType { get; set; }
        public string CarDriveType { get; set; }
        public string CarBodyType { get; set; }
        public string CarColor { get; set; }

        // Стоимость
        public decimal OrderSalePrice { get; set; }

        // Форматированные значения
        public string FormattedPrice => OrderSalePrice.ToString("N0");
        public string ContractDay => ContractDate.Day.ToString();
        public string ContractMonth => GetMonthNameRus(ContractDate.Month);
        public string ContractYear => ContractDate.Year.ToString();

        private string GetMonthNameRus(int month)
        {
            string[] months =
        {
            "января", "февраля", "марта", "апреля", "мая", "июня",
            "июля", "августа", "сентября", "октября", "ноября", "декабря"
        };
            return month >= 1 && month <= 12 ? months[month - 1] : "";
        }
    }
}
