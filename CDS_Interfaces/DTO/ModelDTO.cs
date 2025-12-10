using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.DTO
{
    public class ModelDTO
    {
        public int Id { get; set; }
        public string? ModelName { get; set; }
        public int? BrandId { get; set; }

        public ModelDTO() { }
        public ModelDTO(Model m)
        {
            Id = m.Id;
            ModelName = m.ModelName;
            BrandId = m.BrandId;
        }
    }
}
