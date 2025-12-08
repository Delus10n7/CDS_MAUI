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
    public class ServiceContractsRepositorySQL : IServiceContractsRepository
    {
        private SqlDbContext db;

        public ServiceContractsRepositorySQL(SqlDbContext dbContext)
        {
            this.db = dbContext;
        }

        public List<ServiceContracts> GetList()
        {
            return db.ServiceContracts
                .Include(sc => sc.Client)
                .Include(sc => sc.Manager)
                .Include(sc => sc.SelectedService)
                    .ThenInclude(ss => ss.AdditionalService)
                .ToList();
        }

        public ServiceContracts GetItem(int id)
        {
            return db.ServiceContracts
                .Include(sc => sc.Client)
                .Include(sc => sc.Manager)
                .Include(sc => sc.SelectedService)
                    .ThenInclude(ss => ss.AdditionalService)
                .FirstOrDefault(sc => sc.Id == id);
        }

        public void Create(ServiceContracts contract)
        {
            db.ServiceContracts.Add(contract);
        }

        public void Update(ServiceContracts contract)
        {
            db.Entry(contract).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            ServiceContracts contract = db.ServiceContracts.Find(id);
            if (contract != null)
                db.ServiceContracts.Remove(contract);
        }

        public List<ServiceContracts> GetContractsByClient(int clientId)
        {
            return db.ServiceContracts
                .Where(sc => sc.ClientId == clientId)
                .Include(sc => sc.SelectedService)
                .ToList();
        }

        public decimal GetTotalRevenue(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = db.ServiceContracts.AsQueryable();

            if (startDate.HasValue)
            {
                var start = DateOnly.FromDateTime((DateTime)startDate);
                query = query.Where(sc => sc.SaleDate >= start);
            }
            if (endDate.HasValue)
            {
                var end = DateOnly.FromDateTime((DateTime)endDate);
                query = query.Where(sc => sc.SaleDate <= end);
            }

            return query.Sum(sc => sc.TotalPrice.GetValueOrDefault(0));
        }
    }
}
