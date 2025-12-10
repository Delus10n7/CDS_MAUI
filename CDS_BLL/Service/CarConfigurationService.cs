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
    public class CarConfigurationService : ICarConfigurationService
    {
        private IDbRepos db;
        public CarConfigurationService(IDbRepos repos)
        {
            this.db = repos;
        }
        public List<CarConfigurationDTO> GetAllCarConfigurations()
        {
            return db.CarConfigurations.GetList().Select(i => new CarConfigurationDTO(i)).ToList();
        }
        public CarConfigurationDTO GetCarConfiguration(int Id)
        {
            var config = db.CarConfigurations.GetItem(Id);

            if (config == null)
            {
                throw new ArgumentException($"Конфигурация с id {Id} не найдена!");
            }

            return new CarConfigurationDTO(config);
        }
        public void CreateCarConfiguration(CarConfigurationDTO c)
        {
            db.CarConfigurations.Create(new CarConfiguration
            {
                EngineVolume = c.EngineVolume,
                EnginePower = c.EnginePower,
                ModelId = c.ModelId,
                BodyTypeId = c.BodyTypeId,
                EngineTypeId = c.EngineTypeId,
                TransmissionTypeId = c.TransmissionTypeId,
                DriveTypeId = c.DriveTypeId,
                OtherDetails = c.OtherDetails
            });
            Save();
        }
        public void UpdateCarConfiguration(CarConfigurationDTO c)
        {
            var config = db.CarConfigurations.GetItem(c.Id);

            if (config == null)
            {
                throw new ArgumentException($"Конфигурация с id {c.Id} не найдена!");
            }

            config.EngineVolume = c.EngineVolume;
            config.EnginePower = c.EnginePower;
            config.ModelId = c.ModelId;
            config.BodyTypeId = c.BodyTypeId;
            config.EngineTypeId = c.EngineTypeId;
            config.TransmissionTypeId = c.TransmissionTypeId;
            config.DriveTypeId = c.DriveTypeId;
            config.OtherDetails = c.OtherDetails;

            db.CarConfigurations.Update(config);
            Save();
        }
        public void DeleteCarConfiguration(int Id)
        {
            var c = db.CarConfigurations.GetItem(Id);
            if (c != null)
            {
                db.CarConfigurations.Delete(c.Id);
                Save();
            }
        }
        public bool Save()
        {
            return db.Save() > 0;
        }
    }
}
