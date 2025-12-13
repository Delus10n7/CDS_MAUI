using CDS_DAL.Context;
using CDS_DomainModel.Entities;
using CDS_Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DriveType = CDS_DomainModel.Entities.DriveType;

namespace CDS_DAL.RepositorySQL
{
    public class DriveTypeRepositorySQL : IRepository<DriveType>
    {
        private SqlDbContext db;

        public DriveTypeRepositorySQL(SqlDbContext dbContext)
        {
            this.db = dbContext;
        }

        public List<DriveType> GetList()
        {
            return db.DriveType.ToList();
        }

        public DriveType GetItem(int id)
        {
            return db.DriveType.FirstOrDefault(i => i.Id == id);
        }

        public void Create(DriveType item)
        {
            db.DriveType.Add(item);
        }

        public void Update(DriveType item)
        {
            db.DriveType.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            DriveType driveType = db.DriveType.Find(id);
            if (driveType != null)
                db.DriveType.Remove(driveType);
        }
    }
}
