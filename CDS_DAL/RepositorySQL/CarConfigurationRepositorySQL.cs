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
    public class CarConfigurationRepositorySQL : ICarConfigurationRepository
    {
        private SqlDbContext db;

        public CarConfigurationRepositorySQL(SqlDbContext dbContext)
        {
            this.db = dbContext;
        }

        public List<CarConfiguration> GetList()
        {
            return db.CarConfiguration
                .Include(cc => cc.Model)
                    .ThenInclude(m => m.Brand)
                .Include(cc => cc.BodyType)
                .Include(cc => cc.EngineType)
                .Include(cc => cc.TransmissionType)
                .Include(cc => cc.DriveType)
                .Include(cc => cc.Cars)
                .ToList();
        }

        public CarConfiguration GetItem(int id)
        {
            return db.CarConfiguration
                .Include(cc => cc.Model)
                    .ThenInclude(m => m.Brand)
                .Include(cc => cc.BodyType)
                .Include(cc => cc.EngineType)
                .Include(cc => cc.TransmissionType)
                .Include(cc => cc.DriveType)
                .Include(cc => cc.Cars)
                .FirstOrDefault(cc => cc.Id == id);
        }

        public void Create(CarConfiguration configuration)
        {
            db.CarConfiguration.Add(configuration);
        }

        public void Update(CarConfiguration configuration)
        {
            db.Entry(configuration).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            CarConfiguration configuration = db.CarConfiguration.Find(id);
            if (configuration != null)
                db.CarConfiguration.Remove(configuration);
        }

        public List<CarConfiguration> GetConfigurationsByModel(int modelId)
        {
            return db.CarConfiguration
                .Where(cc => cc.ModelId == modelId)
                .Include(cc => cc.BodyType)
                .Include(cc => cc.EngineType)
                .Include(cc => cc.TransmissionType)
                .Include(cc => cc.DriveType)
                .ToList();
        }

        public List<CarConfiguration> GetConfigurationsByBrand(int brandId)
        {
            return db.CarConfiguration
                .Where(cc => cc.Model.BrandId == brandId)
                .Include(cc => cc.Model)
                    .ThenInclude(m => m.Brand)
                .Include(cc => cc.BodyType)
                .Include(cc => cc.EngineType)
                .Include(cc => cc.TransmissionType)
                .Include(cc => cc.DriveType)
                .ToList();
        }

        public List<CarConfiguration> SearchConfigurations(
            int? bodyTypeId = null,
            int? engineTypeId = null,
            int? transmissionTypeId = null,
            int? driveTypeId = null,
            decimal? minEngineVolume = null,
            decimal? maxEngineVolume = null,
            int? minEnginePower = null,
            int? maxEnginePower = null)
        {
            var query = db.CarConfiguration.AsQueryable();

            if (bodyTypeId.HasValue)
                query = query.Where(cc => cc.BodyTypeId == bodyTypeId.Value);

            if (engineTypeId.HasValue)
                query = query.Where(cc => cc.EngineTypeId == engineTypeId.Value);

            if (transmissionTypeId.HasValue)
                query = query.Where(cc => cc.TransmissionTypeId == transmissionTypeId.Value);

            if (driveTypeId.HasValue)
                query = query.Where(cc => cc.DriveTypeId == driveTypeId.Value);

            if (minEngineVolume.HasValue)
                query = query.Where(cc => cc.EngineVolume >= minEngineVolume.Value);

            if (maxEngineVolume.HasValue)
                query = query.Where(cc => cc.EngineVolume <= maxEngineVolume.Value);

            if (minEnginePower.HasValue)
                query = query.Where(cc => cc.EnginePower >= minEnginePower.Value);

            if (maxEnginePower.HasValue)
                query = query.Where(cc => cc.EnginePower <= maxEnginePower.Value);

            return query
                .Include(cc => cc.Model)
                    .ThenInclude(m => m.Brand)
                .Include(cc => cc.BodyType)
                .Include(cc => cc.EngineType)
                .Include(cc => cc.TransmissionType)
                .Include(cc => cc.DriveType)
                .ToList();
        }

        public CarConfiguration GetFullConfigurationDetails(int configurationId)
        {
            return db.CarConfiguration
                .Include(cc => cc.Model)
                    .ThenInclude(m => m.Brand)
                .Include(cc => cc.BodyType)
                .Include(cc => cc.EngineType)
                .Include(cc => cc.TransmissionType)
                .Include(cc => cc.DriveType)
                .Include(cc => cc.Cars)
                    .ThenInclude(c => c.Condition)
                .Include(cc => cc.Cars)
                    .ThenInclude(c => c.Availability)
                .FirstOrDefault(cc => cc.Id == configurationId);
        }

        public Dictionary<string, int> GetConfigurationStats()
        {
            var stats = new Dictionary<string, int>
            {
                ["TotalConfigurations"] = db.CarConfiguration.Count(),
                ["ConfigurationsWithCars"] = db.CarConfiguration.Count(cc => cc.Cars.Any()),
                ["UniqueBodyTypes"] = db.CarConfiguration.Select(cc => cc.BodyTypeId).Distinct().Count(),
                ["UniqueEngineTypes"] = db.CarConfiguration.Select(cc => cc.EngineTypeId).Distinct().Count()
            };

            return stats;
        }

        public List<CarConfiguration> GetPopularConfigurations(int count = 10)
        {
            return db.CarConfiguration
                .Where(cc => cc.Cars.Any())
                .OrderByDescending(cc => cc.Cars.Count)
                .Take(count)
                .Include(cc => cc.Model)
                    .ThenInclude(m => m.Brand)
                .Include(cc => cc.BodyType)
                .Include(cc => cc.Cars)
                .ToList();
        }

        public bool ConfigurationExists(int modelId, int bodyTypeId, int engineTypeId, int transmissionTypeId, int driveTypeId)
        {
            return db.CarConfiguration
                .Any(cc => cc.ModelId == modelId
                        && cc.BodyTypeId == bodyTypeId
                        && cc.EngineTypeId == engineTypeId
                        && cc.TransmissionTypeId == transmissionTypeId
                        && cc.DriveTypeId == driveTypeId);
        }
    }
}
