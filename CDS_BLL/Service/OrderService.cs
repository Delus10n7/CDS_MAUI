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

        public void DeleteOrder(int Id)
        {
            if (db.Orders.GetItem(Id) != null)
            {
                db.Orders.Delete(Id);
                db.Save();
            }
        }
    }
}
