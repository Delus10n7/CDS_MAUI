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
    public class ModelRepositorySQL : IModelRepository
    {
        private SqlDbContext db;

        public ModelRepositorySQL(SqlDbContext dbContext)
        {
            this.db = dbContext;
        }

        public List<Model> GetList()
        {
            return db.Model
                .Include(m => m.Brand)
                .ToList();
        }

        public Model GetItem(int id)
        {
            return db.Model
                .Include(m => m.Brand)
                .FirstOrDefault(m => m.Id == id);
        }

        public void Create(Model model)
        {
            db.Model.Add(model);
        }

        public void Update(Model model)
        {
            db.Entry(model).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Model model = db.Model.Find(id);
            if (model != null)
                db.Model.Remove(model);
        }

        public List<Model> GetModelsByBrand(int brandId)
        {
            return db.Model
                .Where(m => m.BrandId == brandId)
                .Include(m => m.Brand)
                .ToList();
        }
    }
}
