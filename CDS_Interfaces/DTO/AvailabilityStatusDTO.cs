using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.DTO
{
    public class AvailabilityStatusDTO
    {
        public int Id { get; set; }
        public string AvailabilityName { get; set; } = null!;

        public AvailabilityStatusDTO() { }
        public AvailabilityStatusDTO(AvailabilityStatus a)
        {
            Id = a.Id;
            AvailabilityName = a.AvailabilityName;
        }
    }
}
