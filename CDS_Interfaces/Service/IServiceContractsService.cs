using CDS_Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.Service
{
    public interface IServiceContractsService
    {
        List<ServiceContractDTO> GetAllServiceContracts();
        ServiceContractDTO GetServiceContract(int Id);
        void CreateServiceContract(ServiceContractDTO s);
        void UpdateServiceContract(ServiceContractDTO s);
        void DeleteServiceContract(int Id);
        bool Save();
    }
}
