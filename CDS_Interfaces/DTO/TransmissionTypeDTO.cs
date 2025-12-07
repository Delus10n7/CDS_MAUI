using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.DTO
{
    public class TransmissionTypeDTO
    {
        public int Id { get; set; }
        public string TransmissionName { get; set; } = null!;

        public TransmissionTypeDTO() { }
        public TransmissionTypeDTO(TransmissionType t)
        {
            Id = t.Id;
            TransmissionName = t.TransmissionName;
        }
    }
}
