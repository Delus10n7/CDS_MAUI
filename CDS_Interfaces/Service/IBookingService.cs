using CDS_Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.Service
{
    public interface IBookingService
    {
        List<BookingDTO> GetAllBookings();
        List<BookingDTO> GetAllUserBookings(int UserId);
        List<BookingDTO> GetAllCarBookings(int CarId);
        BookingDTO GetBooking(int Id);
        void CreateBooking(BookingDTO b);
        void UpdateBooking(BookingDTO b);
        void DeleteBooking(int Id);
        bool Save();
    }
}
