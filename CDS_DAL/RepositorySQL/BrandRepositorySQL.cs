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
    public class BrandRepositorySQL : IBrandRepository
    {
        private SqlDbContext db;

        public BrandRepositorySQL(SqlDbContext dbContext)
        {
            this.db = dbContext;
        }

        public List<Brand> GetList()
        {
            return db.Brand
                .Include(b => b.Country)
                .ToList();
        }

        public Brand GetItem(int id)
        {
            return db.Brand
                .Include(b => b.Country)
                .FirstOrDefault(b => b.Id == id);
        }

        public void Create(Brand brand)
        {
            db.Brand.Add(brand);
        }

        public void Update(Brand brand)
        {
            db.Entry(brand).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Brand brand = db.Brand.Find(id);
            if (brand != null)
                db.Brand.Remove(brand);
        }

        public List<Brand> GetBrandsByCountry(int countryId)
        {
            return db.Brand
                .Where(b => b.CountryId == countryId)
                .Include(b => b.Country)
                .ToList();
        }
    }
}
