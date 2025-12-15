using CDS_Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.Service
{
    public interface IOrderService
    {
        List<OrderDTO> GetAllOrders();
        List<OrderStatusDTO> GetAllOrderStatuses();
        OrderDTO GetOrder(int Id);
        void CreateOrder(OrderDTO o);
        void UpdateOrder(OrderDTO o);
        void DeleteOrder(int Id);
    }
}
