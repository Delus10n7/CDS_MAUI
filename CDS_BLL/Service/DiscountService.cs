using CDS_Interfaces.DTO;
using CDS_Interfaces.Repository;
using CDS_Interfaces.Service;
using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_BLL.Service
{
    public class DiscountService : IDiscountService
    {
        private IDbRepos db;
        public DiscountService(IDbRepos repos)
        {
            this.db = repos;
        }
        public List<DiscountDTO> GetAllDicsounts()
        {
            return db.Discounts.GetList().Select(i => new DiscountDTO(i)).ToList();
        }
        public DiscountDTO GetDiscount(int Id)
        {
            var discount = db.Discounts.GetItem(Id);

            if (discount == null)
            {
                throw new ArgumentException($"Скидка с id {Id} не найдена!");
            }

            return new DiscountDTO(discount);
        }
        public void CreateDiscount(DiscountDTO d)
        {
            db.Discounts.Create(new Discount
            {
                ModelId = d.ModelId,
                BrandId = d.BrandId,
                ClientId = d.ClientId,
                DiscountName = d.DiscountName,
                DiscountPercent = d.DiscountPercent,
                DiscountTypeId = d.DiscountTypeId,
                StartDate = d.StartDate,
                EndDate = d.EndDate,
                IsActive = d.IsActive
            });
            Save();
        }
        public void UpdateDiscount(DiscountDTO d)
        {
            var discount = db.Discounts.GetItem(d.Id);

            if (discount == null)
            {
                throw new ArgumentException($"Скидка с id {d.Id} не найдена!");
            }

            discount.ModelId = d.ModelId;
            discount.BrandId = d.BrandId;
            discount.ClientId = d.ClientId;
            discount.DiscountName = d.DiscountName;
            discount.DiscountPercent = d.DiscountPercent;
            discount.DiscountTypeId = d.DiscountTypeId;
            discount.StartDate = d.StartDate;
            discount.EndDate = d.EndDate;
            discount.IsActive = d.IsActive;

            db.Discounts.Update(discount);
            Save();
        }
        public void DeleteDiscount(int Id)
        {
            var d = db.Discounts.GetItem(Id);
            if (d != null)
            {
                db.Discounts.Delete(d.Id);
                Save();
            }
        }
        public bool Save()
        {
            return db.Save() > 0;
        }
    }
}
