using CDS_Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.Service
{
    public interface IUserService
    {
        List<CustomerDTO> GetAllCustomers();
        CustomerDTO GetCustomer(int Id);
        List<ManagerDTO> GetAllManagers();
        ManagerDTO GetManager(int Id);
        List<AdministratorDTO> GetAllAdministrators();
        AdministratorDTO GetAdministrator(int Id);
        void CreateUser(CustomerDTO u);
        void CreateUser(ManagerDTO u);
        void CreateUser(AdministratorDTO u);
        void UpdateUser(CustomerDTO u);
        void UpdateUser(ManagerDTO u);
        void UpdateUser(AdministratorDTO u);
        void DeleteUser(int Id);
        bool Save();
    }
}
