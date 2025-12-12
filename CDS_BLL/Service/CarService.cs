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
    public class CarService : ICarService
    {
        private IDbRepos db;
        public CarService(IDbRepos repos)
        {
            this.db = repos;
        }
        public List<CarDTO> GetAllCars()
        {
            return db.Cars.GetList().Select(i => new CarDTO(i)).ToList();
        }
        public CarDTO GetCar(int Id)
        {
            var car = db.Cars.GetItem(Id);

            if (car == null)
            {
                throw new ArgumentException($"Машина с id {Id} не найдена!");
            }

            return new CarDTO(car);
        }
        public void CreateCar(CarDTO c)
        {
            db.Cars.Create(new Cars()
            {
                VIN = c.VIN,
                Mileage = c.Mileage,
                ReleaseYear = c.ReleaseYear,
                Price = c.Price,
                CarColor = c.CarColor,
                Photo = c.Photo,
                ConditionId = c.ConditionId,
                AvailabilityId = c.AvailabilityId,
                ConfigurationId = c.ConfigurationId
            });
            Save();
        }
        public void UpdateCar(CarDTO c)
        {
            Cars car = db.Cars.GetItem(c.Id);

            if (car == null)
            {
                throw new ArgumentException($"Машина с id {c.Id} не найдена!");
            }

            car.VIN = c.VIN;
            car.Mileage = c.Mileage;
            car.ReleaseYear = c.ReleaseYear;
            car.Price = c.Price;
            car.CarColor = c.CarColor;
            car.Photo = c.Photo;
            car.ConditionId = c.ConditionId;
            car.AvailabilityId = c.AvailabilityId;
            car.ConfigurationId = c.ConfigurationId;

            db.Cars.Update(car);
            Save();
        }
        public void DeleteCar(int Id)
        {
            Cars c = db.Cars.GetItem(Id);
            if (c != null)
            {
                db.Cars.Delete(c.Id);
                Save();
            }
        }
        public bool Save()
        {
            return db.Save() > 0;
        }
    }
}
