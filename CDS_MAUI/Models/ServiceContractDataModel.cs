using CDS_Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_MAUI.Models
{
    public class ServiceContractDataModel
    {
        public string CarDealershipCity { get; set; } = "Иваново";
        public DateTime ContractDate { get; set; } = DateTime.Now;

        public string? CustomerName { get; set; }
        public string? ManagerName { get; set; }
        public decimal? Price { get; set; }
        public ObservableCollection<AdditionalServiceItemModel> AdditionalServiceItems { get; set; } = new ObservableCollection<AdditionalServiceItemModel>();

        public string FormattedPrice => Price?.ToString("N0") ?? "";
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

        public ServiceContractDataModel() { }

        public ServiceContractDataModel(ServiceContractDTO s)
        {
            CustomerName = s.CustomerName;
            ManagerName = s.ManagerName;
            Price = s.TotalPrice;

            foreach (var ss in s.SelectedServices)
            {
                AdditionalServiceItems.Add(new AdditionalServiceItemModel(ss.AdditionalServiceName, ss.Quantity, ss.AdditionalServicePrice));
            }
        }
    }
}
