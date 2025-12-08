using CDS_DomainModel.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CDS_DAL.Context;
using CDS_Interfaces.Repository;

namespace CDS_DAL.RepositorySQL
{
    public class UsersRepositorySQL : IUsersRepository
    {
        private SqlDbContext db;

        public UsersRepositorySQL(SqlDbContext dbContext)
        {
            this.db = dbContext;
        }

        public List<UserBase> GetList()
        {
            return db.Users.ToList();
        }

        public UserBase GetItem(int id)
        {
            return db.Users.Find(id);
        }

        public void Create(UserBase user)
        {
            db.Users.Add(user);
        }

        public void Update(UserBase user)
        {
            db.Entry(user).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            UserBase user = db.Users.Find(id);
            if (user != null)
                db.Users.Remove(user);
        }

        public List<Customer> GetCustomers()
        {
            return db.Users.OfType<Customer>().ToList();
        }

        public List<Manager> GetManagers()
        {
            return db.Users.OfType<Manager>().ToList();
        }

        public List<Administrator> GetAdministrators()
        {
            return db.Users.OfType<Administrator>().ToList();
        }

        public UserBase GetByLogin(string login)
        {
            return db.Users.FirstOrDefault(u => u.UserLogin == login);
        }

        public UserBase GetByEmail(string email)
        {
            return db.Users.FirstOrDefault(u => u.Email == email);
        }
    }
}
