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
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? VIN { get; set; }
        public string? CustomerName { get; set; }
        public string? ManagerName { get; set; }
        public decimal? Price { get; set; }
        public DateOnly? Date { get; set; }
        public string? Status { get; set; }

        public string DisplayInfo => $"{Brand} {Model}";
        public string FormattedDate => Date.ToString();
        public string FormattedPrice => $"{Price:N0} руб.";
        public string ShortDate => Date.ToString();

        public OrderModel(OrderDTO o)
        {
            Brand = o.BrandName;
            Model = o.ModelName;
            VIN = o.VIN;
            CustomerName = o.CustomerName;
            ManagerName = o.ManagerName;
            Price = o.Price;
            Date = o.OrderDate;
            Status = o.OrderStatus;
        }
    }
}
