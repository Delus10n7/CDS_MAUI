using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.Repository
{
    public interface IDiscountRepository : IRepository<Discount>
    {
        List<Discount> GetActiveDiscounts();
        List<Discount> GetDiscountsByClient(int clientId);
        List<Discount> GetDiscountsByModel(int modelId);
    }
}
