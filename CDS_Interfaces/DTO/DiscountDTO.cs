using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.DTO
{
    public class DiscountDTO
    {
        public int Id { get; set; }
        public int? ModelId { get; set; }
        public int? BrandId { get; set; }
        public int? ClientId { get; set; }
        public string? DiscountName { get; set; }
        public decimal? DiscountPercent { get; set; }
        public int? DiscountTypeId { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public bool? IsActive { get; set; }

        public DiscountDTO() { }

        public DiscountDTO(Discount d)
        {
            Id = d.Id;
            ModelId = d.ModelId;
            BrandId = d.BrandId;
            ClientId = d.ClientId;
            DiscountName = d.DiscountName;
            DiscountPercent = d.DiscountPercent;
            DiscountTypeId = d.DiscountTypeId;
            StartDate = d.StartDate;
            EndDate = d.EndDate;
            IsActive = d.IsActive;
        }
    }
}
