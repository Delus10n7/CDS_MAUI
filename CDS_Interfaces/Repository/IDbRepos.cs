using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DriveType = CDS_DomainModel.Entities.DriveType;

namespace CDS_Interfaces.Repository
{
    public interface IDbRepos
    {
        IUsersRepository Users { get; }
        ICarsRepository Cars { get; }
        IOrdersRepository Orders { get; }
        IBrandRepository Brands { get; }
        IModelRepository Models { get; }
        ICarConfigurationRepository CarConfigurations { get; }
        IDiscountRepository Discounts { get; }
        IBookingRepository Bookings { get; }
        IServiceContractsRepository ServiceContracts { get; }
        IReportRepository Reports { get; }
        IRepository<BodyType> BodyTypes { get; }
        IRepository<EngineType> EngineTypes { get; }
        IRepository<TransmissionType> TransmissionTypes { get; }
        IRepository<DriveType> DriveTypes { get; }
        IRepository<OrderStatus> OrderStatuses { get; }
        IRepository<AdditionalService> AdditionalServices { get; }
        int Save();
        void Dispose();
    }
}
