using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.DTO
{
    public class BrandDTO
    {
        public int Id { get; set; }
        public string? BrandName { get; set; }
        public int? CountryId { get; set; }

        public BrandDTO() { }
        public BrandDTO(Brand b)
        {
            Id = b.Id;
            BrandName = b.BrandName;
            CountryId = b.CountryId;
        }
    }
}
