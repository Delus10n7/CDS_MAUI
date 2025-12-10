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
        OrderDTO GetOrder(int Id);

        void DeleteOrder(int Id);
    }
}
