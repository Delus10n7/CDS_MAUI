using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.DTO
{
    public class ManagerDTO : UserBaseDTO
    {
        public ManagerDTO() { }
        public ManagerDTO(UserBase user) : base(user) { }
    }
}
