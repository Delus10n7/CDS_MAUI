using CDS_Interfaces.DTO;
using CDS_Interfaces.Repository;
using CDS_Interfaces.Service;
using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_BLL.Service
{
    public class BookingService : IBookingService
    {
        private IDbRepos db;
        public BookingService(IDbRepos repos)
        {
            this.db = repos; 
        }
        public List<BookingDTO> GetAllBookings()
        {
            return db.Bookings.GetList().Select(i => new BookingDTO(i)).ToList();
        }
        public List<BookingDTO> GetAllUserBookings(int UserId)
        {
            return db.Bookings.GetList().Where(b => b.UserId == UserId).Select(i => new BookingDTO(i)).ToList();
        }
        public List<BookingDTO> GetAllCarBookings(int CarId)
        {
            return db.Bookings.GetList().Where(b => b.CarId == CarId).Select(i => new BookingDTO(i)).ToList();
        }
        public BookingDTO GetBooking(int Id)
        {
            var booking = db.Bookings.GetItem(Id);

            if (booking == null)
            {
                throw new ArgumentException($"Бронирование с id {Id} не найдено!");
            }

            return new BookingDTO(booking);
        }
        public void CreateBooking(BookingDTO b)
        {
            db.Bookings.Create(new Booking
            {
                UserId = b.UserId,
                CarId = b.CarId,
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                BookingStatusId = b.BookingStatusId
            });
            Save();
        }
        public void UpdateBooking(BookingDTO b)
        {
            Booking booking = db.Bookings.GetItem(b.Id);

            if (booking == null)
            {
                throw new ArgumentException($"Бронирование с id {b.Id} не найдено!");
            }

            booking.UserId = b.UserId;
            booking.CarId = b.CarId;
            booking.StartDate = b.StartDate;
            booking.EndDate = b.EndDate;
            booking.BookingStatusId = b.BookingStatusId;

            db.Bookings.Update(booking);
            Save();
        }
        public void DeleteBooking(int Id)
        {
            var b = db.Bookings.GetItem(Id);
            if (b != null)
            {
                db.Bookings.Delete(b.Id);
                Save();
            }
        }
        public bool Save()
        {
            return db.Save() > 0;
        }
    }
}
