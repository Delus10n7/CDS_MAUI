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
    public class BodyTypeRepositorySQL : IRepository<BodyType>
    {
        private SqlDbContext db;

        public BodyTypeRepositorySQL(SqlDbContext dbContext)
        {
            this.db = dbContext;
        }

        public List<BodyType> GetList()
        {
            return db.BodyType.ToList();
        }

        public BodyType GetItem(int id)
        {
            return db.BodyType.FirstOrDefault(i => i.Id == id);
        }

        public void Create(BodyType item)
        {
            db.BodyType.Add(item);
        }

        public void Update(BodyType item)
        {
            db.BodyType.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            BodyType bodyType = db.BodyType.Find(id);
            if (bodyType != null)
                db.BodyType.Remove(bodyType);
        }
    }
}
