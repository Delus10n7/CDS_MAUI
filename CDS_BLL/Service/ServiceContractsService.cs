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
    public class ServiceContractsService : IServiceContractsService
    {
        private IDbRepos db;
        public ServiceContractsService(IDbRepos repos)
        {
            this.db = repos;
        }
        public List<ServiceContractDTO> GetAllServiceContracts()
        {
            return db.ServiceContracts.GetList().Select(i => new ServiceContractDTO(i)).ToList();
        }
        public List<AdditionalServiceDTO> GetAllAdditionalServices()
        {
            return db.AdditionalServices.GetList().Select(i => new AdditionalServiceDTO(i)).ToList();
        }
        public ServiceContractDTO GetServiceContract(int Id)
        {
            var serviceContract = db.ServiceContracts.GetItem(Id);

            if (serviceContract == null)
            {
                throw new ArgumentException($"Контракт на доп услуги с id {Id} не найден!");
            }

            return new ServiceContractDTO(serviceContract);
        }
        public void CreateServiceContract(ServiceContractDTO s)
        {
            db.ServiceContracts.Create(new ServiceContracts
            {
                ClientId = s.ClientId,
                ManagerId = s.ManagerId,
                TotalPrice = s.TotalPrice,
                SaleDate = s.SaleDate
            });
            Save();
        }
        public void CreateSelectedService(SelectedServiceDTO s)
        {
            db.SelectedServices.Create(new SelectedService
            {
                ServiceContractId = s.ServiceContractId,
                AdditionalServiceId = s.AdditionalServiceId,
                Quantity = s.Quantity,
                TotalPrice = s.TotalPrice
            });
            Save();
        }
        public void UpdateServiceContract(ServiceContractDTO s)
        {
            var serviceContract = db.ServiceContracts.GetItem(s.Id);

            if (serviceContract == null)
            {
                throw new ArgumentException($"Контракт на доп услуги с id {s.Id} не найден!");
            }

            serviceContract.ClientId = s.ClientId;
            serviceContract.ManagerId = s.ManagerId;
            serviceContract.TotalPrice = s.TotalPrice;
            serviceContract.SaleDate = s.SaleDate;

            db.ServiceContracts.Update(serviceContract);
            Save();
        }
        public void DeleteServiceContract(int Id)
        {
            var s = db.ServiceContracts.GetItem(Id);
            if (s != null)
            {
                db.ServiceContracts.Delete(s.Id);
                Save();
            }
        }
        public bool Save()
        {
            return db.Save() > 0;
        }
    }
}
