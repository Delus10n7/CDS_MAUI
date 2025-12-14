using CDS_Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_MAUI.Models
{
    public class CustomerModel
    {
        public int? Id { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

        public CustomerModel(CustomerDTO c)
        {
            Id = c.Id;
            FullName = c.FullName;
            Phone = c.PhoneNumber;
            Email = c.Email;
        }

        public CustomerModel(string FullName, string Phone, string Email, int Id = 0)
        {
            this.Id = Id;
            this.FullName = FullName;
            this.Phone = Phone;
            this.Email = Email;
        }
    }
}
