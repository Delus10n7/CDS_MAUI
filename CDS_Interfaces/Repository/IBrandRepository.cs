using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.Repository
{
    public interface IBrandRepository : IRepository<Brand>
    {
        List<Brand> GetBrandsByCountry(int countryId);
    }
}
