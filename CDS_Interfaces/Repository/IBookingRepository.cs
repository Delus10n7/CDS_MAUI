using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.Repository
{
    public interface IBookingRepository : IRepository<Booking>
    {
        List<Booking> GetBookingsByUser(int userId);
        List<Booking> GetActiveBookings();
        bool IsCarAvailable(int carId, DateTime date);
    }
}
