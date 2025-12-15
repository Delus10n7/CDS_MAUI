using CDS_Interfaces.DTO;
using CDS_Interfaces.Repository;
using CDS_Interfaces.Service;
using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_BLL.Service
{
    public class OrderService : IOrderService
    {
        private IDbRepos db;
        public OrderService(IDbRepos repos)
        {
            this.db = repos;
        }
        public List<OrderDTO> GetAllOrders()
        {
            return db.Orders.GetList().Select(i => new OrderDTO(i)).ToList();
        }
        public OrderDTO GetOrder(int Id)
        {
            var order = db.Orders.GetItem(Id);

            if (order == null)
            {
                throw new ArgumentException($"Заказ с id {Id} не найден!");
            }

            return new OrderDTO(order);
        }
        public void CreateOrder(OrderDTO o)
        {
            db.Orders.Create(new Orders()
            {
                IsTradeIn = o.IsTradeIn,
                TradeInValue = o.TradeInValue,
                ClientId = o.ClientId,
                ManagerId = o.ManagerId,
                CarId = o.CarId,
                OrderDate = o.OrderDate,
                StatusId = o.StatusId,
                SalePrice = o.SalePrice
            });
            Save();
        }
        public void UpdateOrder(OrderDTO o)
        {
            Orders order = db.Orders.GetItem(o.Id);

            if (order == null)
            {
                throw new ArgumentException($"Заказ с id {o.Id} не найден!");
            }

            order.IsTradeIn = o.IsTradeIn;
            order.TradeInValue = o.TradeInValue;
            order.ClientId = o.ClientId;
            order.ManagerId = o.ManagerId;
            order.CarId = o.CarId;
            order.OrderDate = o.OrderDate;
            order.StatusId = o.StatusId;
            order.SalePrice = o.SalePrice;

            db.Orders.Update(order);
            Save();
        }
        public void DeleteOrder(int Id)
        {
            if (db.Orders.GetItem(Id) != null)
            {
                db.Orders.Delete(Id);
                db.Save();
            }
        }
        public bool Save()
        {
            return db.Save() > 0;
        }
    }
}
