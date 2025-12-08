using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.Repository
{
    public interface ICarConfigurationRepository : IRepository<CarConfiguration>
    {
        List<CarConfiguration> GetConfigurationsByModel(int modelId);
        List<CarConfiguration> GetConfigurationsByBrand(int brandId);
        List<CarConfiguration> SearchConfigurations(
            int? bodyTypeId = null,
            int? engineTypeId = null,
            int? transmissionTypeId = null,
            int? driveTypeId = null,
            decimal? minEngineVolume = null,
            decimal? maxEngineVolume = null,
            int? minEnginePower = null,
            int? maxEnginePower = null);
        CarConfiguration GetFullConfigurationDetails(int configurationId);
        Dictionary<string, int> GetConfigurationStats();
        List<CarConfiguration> GetPopularConfigurations(int count = 10);
        bool ConfigurationExists(int modelId, int bodyTypeId, int engineTypeId, int transmissionTypeId, int driveTypeId);
    }
}
