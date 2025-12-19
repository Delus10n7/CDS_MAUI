using CDS_DAL.Context;
using CDS_DomainModel.Entities;
using CDS_Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DriveType = CDS_DomainModel.Entities.DriveType;

namespace CDS_DAL.RepositorySQL
{
    public class DbReposSQL : IDbRepos
    {
        private SqlDbContext db;
        private UsersRepositorySQL usersRepository;
        private CarsRepositorySQL carsRepository;
        private OrdersRepositorySQL ordersRepository;
        private BrandRepositorySQL brandRepository;
        private ModelRepositorySQL modelRepository;
        private CarConfigurationRepositorySQL carConfigRepository;
        private DiscountRepositorySQL discountRepository;
        private BookingRepositorySQL bookingRepository;
        private ServiceContractsRepositorySQL serviceContractsRepository;
        private ReportRepositorySQL reportRepository;
        private BodyTypeRepositorySQL bodyTypeRepository;
        private EngineTypeRepositorySQL engineTypeRepository;
        private TransmissionTypeRepositorySQL transmissionTypeRepository;
        private DriveTypeRepositorySQL driveTypeRepository;
        private OrderStatusRepositorySQL orderStatusRepository;
        private AdditionalServiceRepositorySQL additionalServiceRepository;

        public DbReposSQL(SqlDbContext context)
        {
            db = context;
        }

        public IUsersRepository Users
        {
            get
            {
                if (usersRepository == null)
                    usersRepository = new UsersRepositorySQL(db);
                return usersRepository;
            }
        }

        public ICarsRepository Cars
        {
            get
            {
                if (carsRepository == null)
                    carsRepository = new CarsRepositorySQL(db);
                return carsRepository;
            }
        }

        public IOrdersRepository Orders
        {
            get
            {
                if (ordersRepository == null)
                    ordersRepository = new OrdersRepositorySQL(db);
                return ordersRepository;
            }
        }

        public IBrandRepository Brands
        {
            get
            {
                if (brandRepository == null)
                    brandRepository = new BrandRepositorySQL(db);
                return brandRepository;
            }
        }

        public IModelRepository Models
        {
            get
            {
                if (modelRepository == null)
                    modelRepository = new ModelRepositorySQL(db);
                return modelRepository;
            }
        }

        public ICarConfigurationRepository CarConfigurations
        {
            get
            {
                if (carConfigRepository == null)
                    carConfigRepository = new CarConfigurationRepositorySQL(db);
                return carConfigRepository;
            }
        }

        public IDiscountRepository Discounts
        {
            get
            {
                if (discountRepository == null)
                    discountRepository = new DiscountRepositorySQL(db);
                return discountRepository;
            }
        }

        public IBookingRepository Bookings
        {
            get
            {
                if (bookingRepository == null)
                    bookingRepository = new BookingRepositorySQL(db);
                return bookingRepository;
            }
        }

        public IServiceContractsRepository ServiceContracts
        {
            get
            {
                if (serviceContractsRepository == null)
                    serviceContractsRepository = new ServiceContractsRepositorySQL(db);
                return serviceContractsRepository;
            }
        }

        public IReportRepository Reports
        {
            get
            {
                if (reportRepository == null)
                    reportRepository = new ReportRepositorySQL(db);
                return reportRepository;
            }
        }

        public IRepository<BodyType> BodyTypes
        {
            get
            {
                if (bodyTypeRepository == null)
                    bodyTypeRepository = new BodyTypeRepositorySQL(db);
                return bodyTypeRepository;
            }
        }

        public IRepository<EngineType> EngineTypes
        {
            get
            {
                if (engineTypeRepository == null)
                    engineTypeRepository = new EngineTypeRepositorySQL(db);
                return engineTypeRepository;
            }
        }

        public IRepository<TransmissionType> TransmissionTypes
        {
            get
            {
                if (transmissionTypeRepository == null)
                    transmissionTypeRepository = new TransmissionTypeRepositorySQL(db);
                return transmissionTypeRepository;
            }
        }

        public IRepository<DriveType> DriveTypes
        {
            get
            {
                if (driveTypeRepository == null)
                    driveTypeRepository = new DriveTypeRepositorySQL(db);
                return driveTypeRepository;
            }
        }

        public IRepository<OrderStatus> OrderStatuses
        {
            get
            {
                if (orderStatusRepository == null)
                    orderStatusRepository = new OrderStatusRepositorySQL(db);
                return orderStatusRepository;
            }
        }

        public IRepository<AdditionalService> AdditionalServices
        {
            get
            {
                if (additionalServiceRepository == null)
                    additionalServiceRepository = new AdditionalServiceRepositorySQL(db);
                return additionalServiceRepository;
            }
        }

        public int Save()
        {
            try
            {
                return db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving changes: {ex.Message}");
                throw;
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
