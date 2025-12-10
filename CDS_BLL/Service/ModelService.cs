using CDS_Interfaces.DTO;
using CDS_Interfaces.Repository;
using CDS_Interfaces.Service;
using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace CDS_BLL.Service
{
    public class ModelService : IModelService
    {
        private IDbRepos db;
        public ModelService(IDbRepos repos)
        {
            this.db = repos;
        }
        public List<ModelDTO> GetAllModels()
        {
            return db.Models.GetList().Select(i => new ModelDTO(i)).ToList();
        }
        public ModelDTO GetModel(int Id)
        {
            var model = db.Models.GetItem(Id);

            if (model == null)
            {
                throw new ArgumentException($"Модель с id {Id} не найдена!");
            }

            return new ModelDTO(model);
        }
        public void CreateModel(ModelDTO m)
        {
            db.Models.Create(new Model
            {
                ModelName = m.ModelName,
                BrandId = m.BrandId
            });
            Save();
        }
        public void UpdateModel(ModelDTO m)
        {
            var model = db.Models.GetItem(m.Id);

            if (model == null)
            {
                throw new ArgumentException($"Модель с id {m.Id} не найдена!");
            }

            model.ModelName = m.ModelName;
            model.BrandId = m.BrandId;

            db.Models.Update(model);
            Save();
        }
        public void DeleteModel(int Id)
        {
            var m = db.Models.GetItem(Id);
            if (m != null)
            {
                db.Models.Delete(m.Id);
                Save();
            }
        }
        public bool Save()
        {
            return db.Save() > 0;
        }
    }
}
