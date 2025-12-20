using CDS_Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_MAUI.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? VIN { get; set; }
        public string? CustomerName { get; set; }
        public string? ManagerName { get; set; }
        public decimal? Price { get; set; }
        public string? FormattedDate { get; set; }
        public string? Status { get; set; }
        public decimal? SalePrice { get; set; }

        public string DisplayInfo => $"{Brand} {Model}";
        public string FormattedPrice => Price?.ToString("N0") + " руб." ?? string.Empty;
        public string FormattedSalePrice => SalePrice?.ToString("N0") + " руб." ?? string.Empty;

        public OrderModel() { }

        public OrderModel(OrderDTO o)
        {
            Id = o.Id;
            Brand = o.BrandName;
            Model = o.ModelName;
            VIN = o.VIN;
            CustomerName = o.CustomerName;
            ManagerName = o.ManagerName;
            Price = o.Price;
            FormattedDate = o.OrderDate?.ToString("dd.MM.yyyy") ?? string.Empty;
            Status = o.OrderStatus;
            SalePrice = o.SalePrice;
        }
    }
}
