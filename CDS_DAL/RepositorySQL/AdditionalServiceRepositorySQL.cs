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
    public class AdditionalServiceRepositorySQL : IRepository<AdditionalService>
    {
        private SqlDbContext db;

        public AdditionalServiceRepositorySQL(SqlDbContext dbContext)
        {
            this.db = dbContext;
        }

        public List<AdditionalService> GetList()
        {
            return db.AdditionalService.ToList();
        }

        public AdditionalService GetItem(int id)
        {
            return db.AdditionalService.FirstOrDefault(i => i.Id == id);
        }

        public void Create(AdditionalService item)
        {
            db.AdditionalService.Add(item);
        }

        public void Update(AdditionalService item)
        {
            db.AdditionalService.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            AdditionalService additionalService = db.AdditionalService.Find(id);
            if (additionalService != null)
                db.AdditionalService.Remove(additionalService);
        }
    }
}
