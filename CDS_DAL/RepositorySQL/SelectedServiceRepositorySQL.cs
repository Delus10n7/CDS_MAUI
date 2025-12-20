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
    public class SelectedServiceRepositorySQL : IRepository<SelectedService>
    {
        private SqlDbContext db;

        public SelectedServiceRepositorySQL(SqlDbContext dbContext)
        {
            this.db = dbContext;
        }

        public List<SelectedService> GetList()
        {
            return db.SelectedService.ToList();
        }

        public SelectedService GetItem(int id)
        {
            return db.SelectedService.FirstOrDefault(i => i.Id == id);
        }

        public void Create(SelectedService item)
        {
            db.SelectedService.Add(item);
        }

        public void Update(SelectedService item)
        {
            db.SelectedService.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            SelectedService selectedService = db.SelectedService.Find(id);
            if (selectedService != null)
                db.SelectedService.Remove(selectedService);
        }
    }
}
