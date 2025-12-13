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
    public class EngineTypeRepositorySQL : IRepository<EngineType>
    {
        private SqlDbContext db;

        public EngineTypeRepositorySQL(SqlDbContext dbContext)
        {
            this.db = dbContext;
        }

        public List<EngineType> GetList()
        {
            return db.EngineType.ToList();
        }

        public EngineType GetItem(int id)
        {
            return db.EngineType.FirstOrDefault(i => i.Id == id);
        }

        public void Create(EngineType item)
        {
            db.EngineType.Add(item);
        }

        public void Update(EngineType item)
        {
            db.EngineType.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            EngineType EngineType = db.EngineType.Find(id);
            if (EngineType != null)
                db.EngineType.Remove(EngineType);
        }
    }
}
