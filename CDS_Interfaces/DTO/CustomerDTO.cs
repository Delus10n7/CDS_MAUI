using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.DTO
{
    public class CustomerDTO : UserBaseDTO
    {
        public CustomerDTO() { }
        public CustomerDTO(UserBase user) : base(user) { }
        public CustomerDTO(string CustomerName, string CustomerPhone, string CustomerEmail, int CreateId = 0)
        {
            Id = 0;
            UserLogin = "client" + CreateId.ToString();
            PasswordHash = "hash_password_" + CreateId.ToString();
            FullName = CustomerName;
            PhoneNumber = CustomerPhone;
            Email = CustomerEmail;
            RoleId = 1;
            IsActive = true;
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }
    }
}
