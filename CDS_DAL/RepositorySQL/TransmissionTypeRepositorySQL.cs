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
    public class TransmissionTypeRepositorySQL : IRepository<TransmissionType>
    {
        private SqlDbContext db;

        public TransmissionTypeRepositorySQL(SqlDbContext dbContext)
        {
            this.db = dbContext;
        }

        public List<TransmissionType> GetList()
        {
            return db.TransmissionType.ToList();
        }

        public TransmissionType GetItem(int id)
        {
            return db.TransmissionType.FirstOrDefault(i => i.Id == id);
        }

        public void Create(TransmissionType item)
        {
            db.TransmissionType.Add(item);
        }

        public void Update(TransmissionType item)
        {
            db.TransmissionType.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            TransmissionType transmissionType = db.TransmissionType.Find(id);
            if (transmissionType != null)
                db.TransmissionType.Remove(transmissionType);
        }
    }
}
