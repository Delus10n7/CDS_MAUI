using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.DTO
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CarId { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public int BookingStatusId { get; set; }

        public CustomerDTO? User { get; set; }
        public CarDTO? Car { get; set; }
        public BookingStatusDTO? BookingStatus { get; set; }

        public BookingDTO() { }

        public BookingDTO(Booking b)
        {
            Id = b.Id;
            UserId = b.UserId;
            CarId = b.CarId;
            StartDate = b.StartDate;
            EndDate = b.EndDate;
            BookingStatusId = b.BookingStatusId;

            if (b.User is Customer customer)
            {
                User = new CustomerDTO(customer);
            }
        }
    }
}
