using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.DTO
{
    public abstract class UserBaseDTO
    {
        public int Id { get; set; }
        public string UserLogin { get; set; } = null!;
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public int RoleId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string RoleName { get; set; } = null!;

        public UserBaseDTO() { }

        public UserBaseDTO(UserBase user)
        {
            Id = user.Id;
            UserLogin = user.UserLogin;
            FullName = user.FullName;
            PhoneNumber = user.PhoneNumber;
            Email = user.Email;
            IsActive = user.IsActive;
            CreatedDate = user.CreatedDate;
            ModifiedDate = user.ModifiedDate;
            RoleName = user.Role?.RoleName ?? "Unknown";
        }
    }
}
