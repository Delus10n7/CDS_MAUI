using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.DTO
{
    public class UserDTO : UserBaseDTO
    {
        public UserDTO() { }
        public UserDTO(UserBase user) : base(user) { }
    }
}
