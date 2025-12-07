using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.DTO
{
    public class AdditionalServiceDTO
    {
        public int Id { get; set; }
        public string? ServiceName { get; set; }
        public decimal? Price { get; set; }

        public AdditionalServiceDTO() { }
        public AdditionalServiceDTO(AdditionalService a)
        {
            Id = a.Id;
            ServiceName = a.ServiceName;
            Price = a.Price;
        }
    }
}
