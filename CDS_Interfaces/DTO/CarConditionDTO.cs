using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.DTO
{
    public class CarConditionDTO
    {
        public int Id { get; set; }
        public string ConditionName { get; set; } = null!;

        public CarConditionDTO() { }
        public CarConditionDTO(CarCondition c)
        {
            Id = c.Id;
            ConditionName = c.ConditionName;
        }
    }
}
