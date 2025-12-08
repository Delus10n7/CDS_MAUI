using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.Repository
{
    public interface ICarsRepository : IRepository<Cars>
    {
        List<Cars> GetAvailableCars();
        List<Cars> GetCarsByBrand(int brandId);
        List<Cars> GetCarsByPriceRange(decimal minPrice, decimal maxPrice);
        void UpdateAvailability(int carId, int availabilityId);
    }
}
