using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.DTO
{
    public class DiscountTypeDTO
    {
        public int Id { get; set; }
        public string TypeName { get; set; } = null!;

        public DiscountTypeDTO() { }
        public DiscountTypeDTO(DiscountType d)
        {
            Id = d.Id;
            TypeName = d.TypeName;
        }
    }
}
