using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.DTO
{
    public class OrderStatusDTO
    {
        public int Id { get; set; }
        public string StatusName { get; set; } = null!;

        public OrderStatusDTO() { }
        public OrderStatusDTO(OrderStatus o)
        {
            Id = o.Id;
            StatusName = o.StatusName;
        }
    }
}
