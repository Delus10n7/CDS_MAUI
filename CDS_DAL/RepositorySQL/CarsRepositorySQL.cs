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
    public class CarsRepositorySQL : ICarsRepository
    {
        private SqlDbContext db;

        public CarsRepositorySQL(SqlDbContext dbContext)
        {
            this.db = dbContext;
        }

        public List<Cars> GetList()
        {
            return db.Cars
                .Include(c => c.Condition)
                .Include(c => c.Availability)
                .Include(c => c.Configuration)
                    .ThenInclude(cc => cc.Model)
                        .ThenInclude(m => m.Brand)
                .Include(cc => cc.Configuration.BodyType)
                .Include(cc => cc.Configuration.EngineType)
                .Include(cc => cc.Configuration.TransmissionType)
                .Include(cc => cc.Configuration.DriveType)
                .ToList();
        }

        public Cars GetItem(int id)
        {
            return db.Cars
                .Include(c => c.Condition)
                .Include(c => c.Availability)
                .Include(c => c.Configuration)
                    .ThenInclude(cc => cc.Model)
                        .ThenInclude(m => m.Brand)
                .Include(cc => cc.Configuration.BodyType)
                .Include(cc => cc.Configuration.EngineType)
                .Include(cc => cc.Configuration.TransmissionType)
                .Include(cc => cc.Configuration.DriveType)
                .FirstOrDefault(c => c.Id == id);
        }

        public void Create(Cars car)
        {
            db.Cars.Add(car);
        }

        public void Update(Cars car)
        {
            db.Entry(car).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Cars car = db.Cars.Find(id);
            if (car != null) db.Cars.Remove(car);
        }

        // Дополнительные методы
        public List<Cars> GetAvailableCars()
        {
            return db.Cars
                .Where(c => c.AvailabilityId == 1) // В наличии
                .Include(c => c.Configuration)
                .ToList();
        }

        public List<Cars> GetCarsByBrand(int brandId)
        {
            return db.Cars
                .Where(c => c.Configuration.Model.BrandId == brandId)
                .Include(c => c.Configuration)
                    .ThenInclude(cc => cc.Model)
                .ToList();
        }

        public List<Cars> GetCarsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            return db.Cars
                .Where(c => c.Price >= minPrice && c.Price <= maxPrice)
                .ToList();
        }

        public void UpdateAvailability(int carId, int availabilityId)
        {
            var car = db.Cars.Find(carId);
            if (car != null)
            {
                car.AvailabilityId = availabilityId;
                db.SaveChanges();
            }
        }
    }
}
