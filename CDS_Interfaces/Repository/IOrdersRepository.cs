using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.Repository
{
    public interface IOrdersRepository : IRepository<Orders>
    {
        List<Orders> GetOrdersByClient(int clientId);
        List<Orders> GetOrdersByStatus(int statusId);
        List<Orders> GetOrdersByDateRange(DateTime startDate, DateTime endDate);
        decimal GetTotalSales(DateTime? startDate = null, DateTime? endDate = null);
    }
}
