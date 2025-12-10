using CDS_Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.Service
{
    public interface IDiscountService
    {
        List<DiscountDTO> GetAllDicsounts();
        DiscountDTO GetDiscount(int Id);
        void CreateDiscount(DiscountDTO d);
        void UpdateDiscount(DiscountDTO d);
        void DeleteDiscount(int Id);
        bool Save();
    }
}
