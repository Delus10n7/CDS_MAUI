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

        public CustomerDTO? Client { get; set; }
        public ManagerDTO? Manager { get; set; }
        public List<SelectedServiceDTO>? SelectedServices { get; set; }

        public ServiceContractDTO() { }

        public ServiceContractDTO(ServiceContracts s)
        {
            Id = s.Id;
            ClientId = s.ClientId;
            ManagerId = s.ManagerId;
            TotalPrice = s.TotalPrice;
            SaleDate = s.SaleDate;

            if (s.Client is Customer customer)
            {
                Client = new CustomerDTO(customer);
            }

            if (s.Manager is Manager manager)
            {
                Manager = new ManagerDTO(manager);
            }
        }
    }
}
