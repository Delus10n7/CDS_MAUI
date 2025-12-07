using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.DTO
{
    public class AdministratorDTO : UserBaseDTO
    {
        public AdministratorDTO() { }
        public AdministratorDTO(UserBase user) : base(user) { }
    }
}
