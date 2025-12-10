using CDS_Interfaces.DTO;
using CDS_Interfaces.Repository;
using CDS_Interfaces.Service;
using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CDS_BLL.Service
{
    public class BrandService : IBrandService
    {
        private IDbRepos db;
        public BrandService(IDbRepos repos)
        {
            this.db = repos;
        }
        public List<BrandDTO> GetAllBrands()
        {
            return db.Brands.GetList().Select(i => new BrandDTO(i)).ToList();
        }
        public BrandDTO GetBrand(int Id)
        {
            var brand = db.Brands.GetItem(Id);

            if (brand == null)
            {
                throw new ArgumentException($"Марка с id {Id} не найдена!");
            }

            return new BrandDTO(brand);
        }
        public void CreateBrand(BrandDTO b)
        {
            db.Brands.Create(new Brand
            {
                BrandName = b.BrandName,
                CountryId = b.CountryId
            });
            Save();
        }
        public void UpdateBrand(BrandDTO b)
        {
            Brand brand = db.Brands.GetItem(b.Id);

            if (brand == null)
            {
                throw new ArgumentException($"Марка с id {b.Id} не найдена!");
            }

            brand.BrandName = b.BrandName;
            b.CountryId = b.CountryId;

            db.Brands.Update(brand);
            Save();
        }
        public void DeleteBrand(int Id)
        {
            Brand b = db.Brands.GetItem(Id);
            if (b != null)
            {
                db.Brands.Delete(b.Id);
                Save();
            }
        }
        public bool Save()
        {
            return db.Save() > 0;
        }
    }
}
