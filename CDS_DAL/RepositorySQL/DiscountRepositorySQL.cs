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
    public class DiscountRepositorySQL : IDiscountRepository
    {
        private SqlDbContext db;

        public DiscountRepositorySQL(SqlDbContext dbContext)
        {
            this.db = dbContext;
        }

        public List<Discount> GetList()
        {
            return db.Discount
                .Include(d => d.Model)
                .Include(d => d.Brand)
                .Include(d => d.Client)
                .Include(d => d.DiscountType)
                .ToList();
        }

        public Discount GetItem(int id)
        {
            return db.Discount
                .Include(d => d.Model)
                .Include(d => d.Brand)
                .Include(d => d.Client)
                .Include(d => d.DiscountType)
                .FirstOrDefault(d => d.Id == id);
        }

        public void Create(Discount discount)
        {
            db.Discount.Add(discount);
        }

        public void Update(Discount discount)
        {
            db.Entry(discount).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Discount discount = db.Discount.Find(id);
            if (discount != null)
                db.Discount.Remove(discount);
        }

        public List<Discount> GetActiveDiscounts()
        {
            var now = DateTime.Now;
            var date = DateOnly.FromDateTime(now);

            return db.Discount
                .Where(d => d.IsActive.GetValueOrDefault(false)
                         && d.StartDate <= date
                         && d.EndDate >= date)
                .Include(d => d.DiscountType)
                .ToList();
        }

        public List<Discount> GetDiscountsByClient(int clientId)
        {
            return db.Discount
                .Where(d => d.ClientId == clientId && d.IsActive.GetValueOrDefault(false))
                .Include(d => d.DiscountType)
                .ToList();
        }

        public List<Discount> GetDiscountsByModel(int modelId)
        {
            return db.Discount
                .Where(d => d.ModelId == modelId && d.IsActive.GetValueOrDefault(false))
                .Include(d => d.DiscountType)
                .ToList();
        }
    }
}
