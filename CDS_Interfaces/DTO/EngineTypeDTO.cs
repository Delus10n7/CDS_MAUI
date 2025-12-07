using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.DTO
{
    public class EngineTypeDTO
    {
        public int Id { get; set; }
        public string EngineName { get; set; } = null!;

        public EngineTypeDTO() { }
        public EngineTypeDTO(EngineType e)
        {
            Id = e.Id;
            EngineName = e.EngineName;
        }
    }
}
