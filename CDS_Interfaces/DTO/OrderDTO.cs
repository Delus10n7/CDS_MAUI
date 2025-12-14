using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public bool? IsTradeIn { get; set; }
        public decimal? TradeInValue { get; set; }
        public int ClientId { get; set; }
        public int ManagerId { get; set; }
        public int? CarId { get; set; }
        public DateOnly? OrderDate { get; set; }
        public int? StatusId { get; set; }

        // Навигационные свойства
        public string? BrandName { get; set; }
        public string? ModelName { get; set; }
        public string? VIN { get; set; }
        public string? CustomerName { get; set; }
        public string? ManagerName { get; set; }
        public string? OrderStatus { get; set; }
        public decimal? Price { get; set; }

        public OrderDTO() { }

        public OrderDTO(Orders o)
        {
            Id = o.Id;
            IsTradeIn = o.IsTradeIn;
            TradeInValue = o.TradeInValue;
            ClientId = o.ClientId;
            ManagerId = o.ManagerId;
            CarId = o.CarId;
            OrderDate = o.OrderDate;
            StatusId = o.StatusId;

            // Навигационные свойства
            BrandName = o.Car.Configuration.Model.Brand.BrandName;
            ModelName = o.Car.Configuration.Model.ModelName;
            VIN = o.Car.VIN;
            CustomerName = o.Client.FullName;
            ManagerName = o.Manager.FullName;
            OrderStatus = o.Status.StatusName;
            Price = o.Car.Price;
        }
    }
}
