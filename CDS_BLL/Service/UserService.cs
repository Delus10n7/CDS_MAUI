using CDS_Interfaces.DTO;
using CDS_Interfaces.Repository;
using CDS_Interfaces.Service;
using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_BLL.Service
{
    public class UserService : IUserService
    {
        private IDbRepos db;
        public UserService(IDbRepos repos)
        {
            this.db = repos;
        }
        public List<CustomerDTO> GetAllCustomers()
        {
            return db.Users.GetCustomers().Select(i => new CustomerDTO(i)).ToList();
        }
        public CustomerDTO GetCustomer(int Id)
        {
            var customer = db.Users.GetItem(Id);

            if (customer == null || customer.RoleId != 1)
            {
                throw new ArgumentException($"Клиент с id {Id} не найден");
            }

            return new CustomerDTO(customer);
        }
        public List<ManagerDTO> GetAllManagers()
        {
            return db.Users.GetManagers().Select(i => new ManagerDTO(i)).ToList();
        }
        public ManagerDTO GetManager(int Id)
        {
            var manager = db.Users.GetItem(Id);

            if (manager == null || manager.RoleId != 2)
            {
                throw new ArgumentException($"Менеджер с id {Id} не найден");
            }

            return new ManagerDTO(manager);
        }
        public List<AdministratorDTO> GetAllAdministrators()
        {
            return db.Users.GetAdministrators().Select(i => new AdministratorDTO(i)).ToList();
        }
        public AdministratorDTO GetAdministrator(int Id)
        {
            var admin = db.Users.GetItem(Id);

            if (admin == null || admin.RoleId != 3)
            {
                throw new ArgumentException($"Администратор с id {Id} не найден");
            }

            return new AdministratorDTO(admin);
        }
        public void CreateUser(CustomerDTO u)
        {
            db.Users.Create(new Customer
            {
                UserLogin = u.UserLogin,
                PasswordHash = u.PasswordHash,
                FullName = u.FullName,
                PhoneNumber = u.PhoneNumber,
                Email = u.Email,
                RoleId = u.RoleId,
                IsActive = u.IsActive,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            });
            Save();
        }
        public void CreateUser(ManagerDTO u)
        {
            db.Users.Create(new Manager
            {
                UserLogin = u.UserLogin,
                PasswordHash = u.PasswordHash,
                FullName = u.FullName,
                PhoneNumber = u.PhoneNumber,
                Email = u.Email,
                RoleId = u.RoleId,
                IsActive = u.IsActive,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            });
            Save();
        }
        public void CreateUser(AdministratorDTO u)
        {
            db.Users.Create(new Administrator
            {
                UserLogin = u.UserLogin,
                PasswordHash = u.PasswordHash,
                FullName = u.FullName,
                PhoneNumber = u.PhoneNumber,
                Email = u.Email,
                RoleId = u.RoleId,
                IsActive = u.IsActive,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            });
            Save();
        }
        public void UpdateUser(CustomerDTO u)
        {
            UserBase user = db.Users.GetItem(u.Id);

            if (user == null)
            {
                throw new ArgumentException($"Пользователь с id {u.Id} не найден");
            }

            user.UserLogin = u.UserLogin;
            user.PasswordHash = u.PasswordHash;
            user.FullName = u.FullName;
            user.PhoneNumber = u.PhoneNumber;
            user.Email = u.Email;
            user.RoleId = u.RoleId;
            user.IsActive = u.IsActive;
            user.CreatedDate = u.CreatedDate;
            user.ModifiedDate = DateTime.Now;

            db.Users.Update(user);
            Save();
        }
        public void UpdateUser(ManagerDTO u)
        {
            UserBase user = db.Users.GetItem(u.Id);

            if (user == null)
            {
                throw new ArgumentException($"Пользователь с id {u.Id} не найден");
            }

            user.UserLogin = u.UserLogin;
            user.PasswordHash = u.PasswordHash;
            user.FullName = u.FullName;
            user.PhoneNumber = u.PhoneNumber;
            user.Email = u.Email;
            user.RoleId = u.RoleId;
            user.IsActive = u.IsActive;
            user.CreatedDate = u.CreatedDate;
            user.ModifiedDate = DateTime.Now;

            db.Users.Update(user);
            Save();
        }
        public void UpdateUser(AdministratorDTO u)
        {
            UserBase user = db.Users.GetItem(u.Id);

            if (user == null)
            {
                throw new ArgumentException($"Пользователь с id {u.Id} не найден");
            }

            user.UserLogin = u.UserLogin;
            user.PasswordHash = u.PasswordHash;
            user.FullName = u.FullName;
            user.PhoneNumber = u.PhoneNumber;
            user.Email = u.Email;
            user.RoleId = u.RoleId;
            user.IsActive = u.IsActive;
            user.CreatedDate = u.CreatedDate;
            user.ModifiedDate = DateTime.Now;

            db.Users.Update(user);
            Save();
        }
        public void DeleteUser(int Id)
        {
            UserBase user = db.Users.GetItem(Id);

            if (user != null)
            {
                db.Users.Delete(user.Id);
                Save();
            }
        }
        public bool Save()
        {
            return db.Save() > 0;
        }
    }
}
