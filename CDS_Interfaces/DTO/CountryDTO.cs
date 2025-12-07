using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.DTO
{
    public class CountryDTO
    {
        public int Id { get; set; }
        public string? CountryName { get; set; }

        public CountryDTO() { }
        public CountryDTO(Country c)
        {
            Id = c.Id;
            CountryName = c.CountryName;
        }
    }
}
