using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.DTO
{
    public class ServiceContractDTO
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int ManagerId { get; set; }
        public decimal? TotalPrice { get; set; }
        public DateOnly? SaleDate { get; set; }

        // Навигационные свойства
        public string? CustomerName { get; set; }
        public string? ManagerName { get; set; }
        public ICollection<SelectedServiceDTO> SelectedServices { get; set; } = new List<SelectedServiceDTO>();

        public ServiceContractDTO() { }

        public ServiceContractDTO(ServiceContracts s)
        {
            Id = s.Id;
            ClientId = s.ClientId;
            ManagerId = s.ManagerId;
            TotalPrice = s.TotalPrice;
            SaleDate = s.SaleDate;

            // Навигационные свойства
            CustomerName = s.Client.FullName;
            ManagerName = s.Manager.FullName;
            foreach(var sc in s.SelectedService)
            {
                SelectedServices.Add(new SelectedServiceDTO(sc));
            }
        }
    }
}
