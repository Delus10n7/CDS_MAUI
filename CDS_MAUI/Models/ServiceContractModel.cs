using CDS_Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_MAUI.Models
{
    public class ServiceContractModel
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public string? ManagerName { get; set; }
        public decimal? Price { get; set; }
        public string? Date { get; set; }
        public ObservableCollection<AdditionalServiceItemModel> AdditionalServiceItems { get; set; } = new ObservableCollection<AdditionalServiceItemModel>();

        public string? FormattedPrice => Price?.ToString("N0") + " руб." ?? string.Empty;
        public string? FormattedCustomerName => "Клиент: " + CustomerName ?? string.Empty;
        public string? FormattedManagerName => "Менеджер: " + ManagerName ?? string.Empty;

        public ServiceContractModel() { }

        public ServiceContractModel(ServiceContractDTO sc)
        {
            Id = sc.Id;
            CustomerName = sc.CustomerName;
            ManagerName = sc.ManagerName;
            Price = sc.TotalPrice;
            Date = sc.SaleDate.ToString();

            foreach(var ss in sc.SelectedServices)
            {
                AdditionalServiceItems.Add(new AdditionalServiceItemModel(ss.AdditionalServiceName, ss.Quantity, ss.AdditionalServicePrice));
            }
        }
    }
}
