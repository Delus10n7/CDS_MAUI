using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.DTO
{
    public class SelectedServiceDTO
    {
        public int Id { get; set; }
        public int ServiceContractId { get; set; }
        public int AdditionalServiceId { get; set; }
        public int? Quantity { get; set; }
        public decimal? TotalPrice { get; set; }

        // Навигационные свойства
        public string? AdditionalServiceName { get; set; }
        public decimal? AdditionalServicePrice { get; set; }

        public SelectedServiceDTO() { }

        public SelectedServiceDTO(SelectedService s)
        {
            Id = s.Id;
            ServiceContractId = s.ServiceContractId;
            AdditionalServiceId = s.AdditionalServiceId;
            Quantity = s.Quantity;
            TotalPrice = s.TotalPrice;

            // Навигационные свойства
            AdditionalServiceName = s.AdditionalService.ServiceName;
            AdditionalServicePrice = s.AdditionalService.Price;
        }
    }
}
