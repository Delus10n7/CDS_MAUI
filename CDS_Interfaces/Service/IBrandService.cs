using CDS_Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.Service
{
    public interface IBrandService
    {
        List<BrandDTO> GetAllBrands();
        BrandDTO GetBrand(int Id);
        void CreateBrand(BrandDTO b);
        void UpdateBrand(BrandDTO b);
        void DeleteBrand(int Id);
        bool Save();
    }
}
