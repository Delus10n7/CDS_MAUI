using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.Repository
{
    public interface IServiceContractsRepository : IRepository<ServiceContracts>
    {
        List<ServiceContracts> GetContractsByClient(int clientId);
        decimal GetTotalRevenue(DateTime? startDate = null, DateTime? endDate = null);

    }
}
