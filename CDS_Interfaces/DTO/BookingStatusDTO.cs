using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.DTO
{
    public class BookingStatusDTO
    {
        public int Id { get; set; }
        public string StatusName { get; set; } = null!;

        public BookingStatusDTO() { }
        public BookingStatusDTO(BookingStatus b)
        {
            Id = b.Id;
            StatusName = b.StatusName;
        }
    }
}
