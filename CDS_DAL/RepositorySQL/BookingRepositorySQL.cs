using CDS_DAL.Context;
using CDS_DomainModel.Entities;
using CDS_Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_DAL.RepositorySQL
{
    public class BookingRepositorySQL : IBookingRepository
    {
        private SqlDbContext db;

        public BookingRepositorySQL(SqlDbContext dbContext)
        {
            this.db = dbContext;
        }

        public List<Booking> GetList()
        {
            return db.Booking
                .Include(b => b.User)
                .Include(b => b.Car)
                .Include(b => b.BookingStatus)
                .ToList();
        }

        public Booking GetItem(int id)
        {
            return db.Booking
                .Include(b => b.User)
                .Include(b => b.Car)
                .Include(b => b.BookingStatus)
                .FirstOrDefault(b => b.Id == id);
        }

        public void Create(Booking booking)
        {
            db.Booking.Add(booking);
        }

        public void Update(Booking booking)
        {
            db.Entry(booking).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Booking booking = db.Booking.Find(id);
            if (booking != null)
                db.Booking.Remove(booking);
        }

        public List<Booking> GetBookingsByUser(int userId)
        {
            return db.Booking
                .Where(b => b.UserId == userId)
                .Include(b => b.Car)
                .Include(b => b.BookingStatus)
                .ToList();
        }

        public List<Booking> GetActiveBookings()
        {
            return db.Booking
                .Where(b => b.BookingStatusId == 1)
                .Include(b => b.User)
                .Include(b => b.Car)
                .ToList();
        }

        public bool IsCarAvailable(int carId, DateTime date)
        {
            var dateOnly = DateOnly.FromDateTime(date);

            return !db.Booking
                .Any(b => b.CarId == carId
                       && b.StartDate <= dateOnly
                       && b.EndDate >= dateOnly
                       && b.BookingStatusId == 1);
        }
    }
}
