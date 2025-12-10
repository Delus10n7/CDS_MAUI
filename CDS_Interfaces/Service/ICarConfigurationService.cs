using CDS_Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.Service
{
    public interface ICarConfigurationService
    {
        List<CarConfigurationDTO> GetAllCarConfigurations();
        CarConfigurationDTO GetCarConfiguration(int Id);
        void CreateCarConfiguration(CarConfigurationDTO c);
        void UpdateCarConfiguration(CarConfigurationDTO c);
        void DeleteCarConfiguration(int Id);
        bool Save();
    }
}
