using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CDS_DomainModel.Entities;
using DriveType = CDS_DomainModel.Entities.DriveType;

namespace CDS_Interfaces.DTO
{
    public class DriveTypeDTO
    {
        public int Id { get; set; }
        public string DriveName { get; set; } = null!;

        public DriveTypeDTO() { }
        public DriveTypeDTO(DriveType d)
        {
            Id = d.Id;
            DriveName = d.DriveName;
        }
    }
}
