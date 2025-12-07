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
        public int? TradeInCarId { get; set; }
        public int ClientId { get; set; }
        public int ManagerId { get; set; }
        public int? CarId { get; set; }
        public DateOnly? OrderDate { get; set; }
        public int? StatusId { get; set; }

        public CustomerDTO? Client { get; set; }
        public ManagerDTO? Manager { get; set; }
        public CarDTO? Car { get; set; }
        public OrderStatusDTO? Status { get; set; }
        public CarDTO? TradeInCar { get; set; }

        public OrderDTO() { }

        public OrderDTO(Orders o)
        {
            Id = o.Id;
            IsTradeIn = o.IsTradeIn;
            TradeInValue = o.TradeInValue;
            TradeInCarId = o.TradeInCarId;
            ClientId = o.ClientId;
            ManagerId = o.ManagerId;
            CarId = o.CarId;
            OrderDate = o.OrderDate;
            StatusId = o.StatusId;

            // TPH автоматически приводит к нужным типам
            if (o.Client is Customer customer)
            {
                Client = new CustomerDTO(customer);
            }

            if (o.Manager is Manager manager)
            {
                Manager = new ManagerDTO(manager);
            }
        }
    }
}
