using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.DTO
{
    public class BodyTypeDTO
    {
        public int Id { get; set; }
        public string BodyName { get; set; } = null!;

        public BodyTypeDTO() { }
        public BodyTypeDTO(BodyType b)
        {
            Id = b.Id;
            BodyName = b.BodyName;
        }
    }
}
