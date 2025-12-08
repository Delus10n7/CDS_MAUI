using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.Repository
{
    public interface IUsersRepository : IRepository<UserBase>
    {
        List<Customer> GetCustomers();
        List<Manager> GetManagers();
        List<Administrator> GetAdministrators();
        UserBase GetByLogin(string login);
        UserBase GetByEmail(string email);
    }
}
