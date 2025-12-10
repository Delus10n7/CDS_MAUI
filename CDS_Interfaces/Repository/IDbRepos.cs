using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        int Save();
        void Dispose();
    }
}
